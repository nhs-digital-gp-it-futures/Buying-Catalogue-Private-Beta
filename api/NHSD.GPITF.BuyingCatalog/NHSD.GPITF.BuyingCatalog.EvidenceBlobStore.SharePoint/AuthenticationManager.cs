using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client.NetCore;
using System.Net;
using System.Threading;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  /// <summary>
  /// This manager class can be used to obtain a SharePointContext object
  /// </summary>
  public sealed class AuthenticationManager : IAuthenticationManager
  {
    private const string SHAREPOINT_PRINCIPAL = "00000003-0000-0ff1-ce00-000000000000";

    private readonly object _tokenLock = new object();
    private readonly ILogger _logger;
    private string _appOnlyAccessToken;

    public AuthenticationManager(ILogger<AuthenticationManager> logger)
    {
      _logger = logger;

      // Set the TLS preference. Needed on some server os's to work when Office 365 removes support for TLS 1.0
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
    }

    #region Authenticating against SharePoint Online using credentials or app-only
    /// <summary>
    /// Returns an app only ClientContext object
    /// </summary>
    /// <param name="siteUrl">Site for which the ClientContext object will be instantiated</param>
    /// <param name="appId">Application ID which is requesting the ClientContext object</param>
    /// <param name="appSecret">Application secret of the Application which is requesting the ClientContext object</param>
    /// <returns>ClientContext to be used by CSOM code</returns>
    public ClientContext GetAppOnlyAuthenticatedContext(
      string siteUrl,
      string appId,
      string appSecret)
    {
      return GetAppOnlyAuthenticatedContext(siteUrl, TokenHelper.GetRealmFromTargetUrl(new Uri(siteUrl)), appId, appSecret);
    }

    /// <summary>
    /// Returns an app only ClientContext object
    /// </summary>
    /// <param name="siteUrl">Site for which the ClientContext object will be instantiated</param>
    /// <param name="realm">Realm of the environment (tenant) that requests the ClientContext object</param>
    /// <param name="appId">Application ID which is requesting the ClientContext object</param>
    /// <param name="appSecret">Application secret of the Application which is requesting the ClientContext object</param>
    /// <param name="acsHostUrl">Azure ACS host, defaults to accesscontrol.windows.net but internal pre-production environments use other hosts</param>
    /// <param name="globalEndPointPrefix">Azure ACS endpoint prefix, defaults to accounts but internal pre-production environments use other prefixes</param>
    /// <returns>ClientContext to be used by CSOM code</returns>
    private ClientContext GetAppOnlyAuthenticatedContext(
      string siteUrl,
      string realm,
      string appId,
      string appSecret,
      string acsHostUrl = "accesscontrol.windows.net",
      string globalEndPointPrefix = "accounts")
    {
      EnsureToken(siteUrl, realm, appId, appSecret, acsHostUrl, globalEndPointPrefix);
      ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(siteUrl, _appOnlyAccessToken);

      return clientContext;
    }
    #endregion

    /// <summary>
    /// Ensure that AppAccessToken is filled with a valid string representation of the OAuth AccessToken. This method will launch handle with token cleanup after the token expires
    /// </summary>
    /// <param name="siteUrl">Site for which the ClientContext object will be instantiated</param>
    /// <param name="realm">Realm of the environment (tenant) that requests the ClientContext object</param>
    /// <param name="appId">Application ID which is requesting the ClientContext object</param>
    /// <param name="appSecret">Application secret of the Application which is requesting the ClientContext object</param>
    /// <param name="acsHostUrl">Azure ACS host, defaults to accesscontrol.windows.net but internal pre-production environments use other hosts</param>
    /// <param name="globalEndPointPrefix">Azure ACS endpoint prefix, defaults to accounts but internal pre-production environments use other prefixes</param>
    private void EnsureToken(
      string siteUrl,
      string realm,
      string appId,
      string appSecret,
      string acsHostUrl,
      string globalEndPointPrefix)
    {
      if (_appOnlyAccessToken == null)
      {
        lock (_tokenLock)
        {
          _logger.LogDebug("AuthenticationManager:EnsureToken --> (siteUrl:{0},realm:{1},appId:{2},appSecret:PRIVATE)", siteUrl, realm, appId);
          if (_appOnlyAccessToken == null)
          {
            TokenHelper.Realm = realm;
            TokenHelper.ServiceNamespace = realm;
            TokenHelper.ClientId = appId;
            TokenHelper.ClientSecret = appSecret;

            if (!string.IsNullOrEmpty(acsHostUrl))
            {
              TokenHelper.AcsHostUrl = acsHostUrl;
            }

            if (globalEndPointPrefix != null)
            {
              TokenHelper.GlobalEndPointPrefix = globalEndPointPrefix;
            }

            var response = TokenHelper.GetAppOnlyAccessToken(SHAREPOINT_PRINCIPAL, new Uri(siteUrl).Authority, realm);
            string token = response.AccessToken;
            ThreadPool.QueueUserWorkItem(obj =>
            {
              try
              {
                _logger.LogDebug($"AuthenticationManager:EnsureToken --> Lease expiration date: {response.ExpiresOn}");

                const double LeaseMarginSeconds = 5 * 60;

                var lease = GetAccessTokenLease(response.ExpiresOn);
                lease = TimeSpan.FromSeconds(lease.TotalSeconds - LeaseMarginSeconds > 0 ?
                  lease.TotalSeconds - LeaseMarginSeconds : lease.TotalSeconds);
                Thread.Sleep(lease);

                _appOnlyAccessToken = null;
              }
              catch (Exception ex)
              {
                _logger.LogError($"AuthenticationManager:EnsureToken --> Problem Determining Token Lease: {ex.Message}");
                _appOnlyAccessToken = null;
              }
            });
            _appOnlyAccessToken = token;
          }
        }
      }
    }

    /// <summary>
    /// Get the access token lease time span.
    /// </summary>
    /// <param name="expiresOn">The ExpiresOn time of the current access token</param>
    /// <returns>Returns a TimeSpan represents the time interval within which the current access token is valid thru.</returns>
    private static TimeSpan GetAccessTokenLease(DateTime expiresOn)
    {
      DateTime now = DateTime.UtcNow;
      DateTime expires = expiresOn.Kind == DateTimeKind.Utc ?
          expiresOn : TimeZoneInfo.ConvertTimeToUtc(expiresOn);
      TimeSpan lease = expires - now;

      return lease;
    }
  }
}
