using Microsoft.SharePoint.Client.NetCore;
using Microsoft.SharePoint.Client.NetCore.Runtime;
using SharePointPnP.IdentityModel.Extensions.S2S.Protocols.OAuth2;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public static class TokenHelper
  {
    #region Properties
    private static string realm = null;
    public static string Realm
    {
      get
      {
        if (string.IsNullOrEmpty(realm))
        {
          return ConfigurationManager.AppSettings.Get("Realm");
        }
        else
        {
          return realm;
        }
      }
      set
      {
        realm = value;
      }
    }

    private static string serviceNamespace = null;
    public static string ServiceNamespace
    {
      get
      {
        if (string.IsNullOrEmpty(serviceNamespace))
        {
          return ConfigurationManager.AppSettings.Get("Realm");
        }
        else
        {
          return serviceNamespace;
        }
      }
      set
      {
        serviceNamespace = value;
      }
    }

    private static string clientId = null;
    public static string ClientId
    {
      get
      {
        if (string.IsNullOrEmpty(clientId))
        {
          return string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ClientId")) ? ConfigurationManager.AppSettings.Get("HostedAppName") : ConfigurationManager.AppSettings.Get("ClientId");
        }
        else
        {
          return clientId;
        }
      }
      set
      {
        clientId = value;
      }
    }

    private static string clientSecret = null;
    public static string ClientSecret
    {
      get
      {
        if (string.IsNullOrEmpty(clientSecret))
        {
          return string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ClientSecret")) ? ConfigurationManager.AppSettings.Get("HostedAppSigningKey") : ConfigurationManager.AppSettings.Get("ClientSecret");
        }
        else
        {
          return clientSecret;
        }
      }
      set
      {
        clientSecret = value;
      }
    }

    private static string acsHostUrl = "accesscontrol.windows.net";
    public static string AcsHostUrl
    {
      get
      {
        if (string.IsNullOrEmpty(acsHostUrl))
        {
          return "accesscontrol.windows.net";
        }
        else
        {
          return acsHostUrl;
        }
      }
      set
      {
        acsHostUrl = value;
      }
    }

    private static string globalEndPointPrefix = "accounts";
    public static string GlobalEndPointPrefix
    {
      get
      {
        if (globalEndPointPrefix == null)
        {
          return "accounts";
        }
        else
        {
          return globalEndPointPrefix;
        }
      }
      set
      {
        globalEndPointPrefix = value;
      }
    }

    private static string hostedAppHostName = null;
    public static string HostedAppHostName
    {
      get
      {
        if (string.IsNullOrEmpty(hostedAppHostName))
        {
          return ConfigurationManager.AppSettings.Get("HostedAppHostName");
        }
        else
        {
          return hostedAppHostName;
        }
      }
      set
      {
        hostedAppHostName = value;
      }
    }
    #endregion

    public static string GetRealmFromTargetUrl(Uri targetApplicationUri)
    {
      WebRequest request = WebRequest.Create(targetApplicationUri.ToString().TrimEnd(new[] { '/' }) + "/_vti_bin/client.svc");
      request.Headers.Add("Authorization: Bearer ");

      try
      {
        using (request.GetResponse())
        {
        }
      }
      catch (WebException e)
      {
        if (e.Response == null)
        {
          return null;
        }

        string bearerResponseHeader = e.Response.Headers["WWW-Authenticate"];
        if (string.IsNullOrEmpty(bearerResponseHeader))
        {
          return null;
        }

        const string bearer = "Bearer realm=\"";
        int bearerIndex = bearerResponseHeader.IndexOf(bearer, StringComparison.Ordinal);
        if (bearerIndex < 0)
        {
          return null;
        }

        int realmIndex = bearerIndex + bearer.Length;

        if (bearerResponseHeader.Length >= realmIndex + 36)
        {
          string targetRealm = bearerResponseHeader.Substring(realmIndex, 36);

          if (Guid.TryParse(targetRealm, out Guid realmGuid))
          {
            return targetRealm;
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Uses the specified access token to create a client context
    /// </summary>
    /// <param name="targetUrl">Url of the target SharePoint site</param>
    /// <param name="accessToken">Access token to be used when calling the specified targetUrl</param>
    /// <returns>A ClientContext ready to call targetUrl with the specified access token</returns>
    public static ClientContext GetClientContextWithAccessToken(
      string targetUrl,
      string accessToken)
    {
      ClientContext clientContext = new ClientContext(targetUrl)
      {
        AuthenticationMode = ClientAuthenticationMode.Anonymous,
        FormDigestHandlingEnabled = false
      };
      clientContext.ExecutingWebRequest +=
        delegate (object oSender, WebRequestEventArgs webRequestEventArgs)
        {
          webRequestEventArgs.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + accessToken;
        };

      return clientContext;
    }

    /// <summary>
    /// Retrieves an app-only access token from ACS to call the specified principal 
    /// at the specified targetHost. The targetHost must be registered for target principal.  If specified realm is 
    /// null, the "Realm" setting in web.config will be used instead.
    /// </summary>
    /// <param name="targetPrincipalName">Name of the target principal to retrieve an access token for</param>
    /// <param name="targetHost">Url authority of the target principal</param>
    /// <param name="targetRealm">Realm to use for the access token's nameid and audience</param>
    /// <returns>An access token with an audience of the target principal</returns>
    public static OAuth2AccessTokenResponse GetAppOnlyAccessToken(
      string targetPrincipalName,
      string targetHost,
      string targetRealm)
    {
      if (targetRealm == null)
      {
        targetRealm = Realm;
      }

      string resource = GetFormattedPrincipal(targetPrincipalName, targetHost, targetRealm);
      string clientId = GetFormattedPrincipal(ClientId, HostedAppHostName, targetRealm);

      OAuth2AccessTokenRequest oauth2Request = OAuth2MessageFactory.CreateAccessTokenRequestWithClientCredentials(clientId, ClientSecret, resource);
      oauth2Request.Resource = resource;

      // Get token
      OAuth2S2SClient client = new OAuth2S2SClient();

      OAuth2AccessTokenResponse oauth2Response;
      try
      {
        oauth2Response = client.Issue(AcsMetadataParser.GetStsUrl(targetRealm), oauth2Request) as OAuth2AccessTokenResponse;
      }
      catch (WebException wex)
      {
        using (StreamReader sr = new StreamReader(wex.Response.GetResponseStream()))
        {
          string responseText = sr.ReadToEnd();
          throw new WebException(wex.Message + " - " + responseText, wex);
        }
      }

      return oauth2Response;
    }

    private static string GetFormattedPrincipal(string principalName, string hostName, string realm)
    {
      if (!string.IsNullOrEmpty(hostName))
      {
        return string.Format(CultureInfo.InvariantCulture, "{0}/{1}@{2}", principalName, hostName, realm);
      }

      return string.Format(CultureInfo.InvariantCulture, "{0}@{1}", principalName, realm);
    }
  }
}
