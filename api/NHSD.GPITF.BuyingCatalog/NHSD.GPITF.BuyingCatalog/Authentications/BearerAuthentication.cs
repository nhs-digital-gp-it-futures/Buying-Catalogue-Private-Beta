using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.Authentications
{
#pragma warning disable CS1591
  public sealed class BearerAuthentication : IBearerAuthentication
  {
    private readonly IUserInfoResponseCache _cache;
    private readonly IConfiguration _config;
    private readonly ILogger<BearerAuthentication> _logger;
    private readonly IUserInfoResponseRetriever _userInfoClient;
    private readonly IContactsDatastore _contactsDatastore;
    private readonly IOrganisationsDatastore _organisationDatastore;
    private readonly bool _logBearerAuth;

    private static TimeSpan Expiry = TimeSpan.FromMinutes(60);

    public BearerAuthentication(
      IUserInfoResponseCache cache,
      IConfiguration config,
      ILogger<BearerAuthentication> logger,
      IUserInfoResponseRetriever userInfoClient,
      IContactsDatastore contactsDatastore,
      IOrganisationsDatastore organisationDatastore)
    {
      _cache = cache;
      _config = config;
      _logger = logger;
      _userInfoClient = userInfoClient;
      _contactsDatastore = contactsDatastore;
      _organisationDatastore = organisationDatastore;
      _logBearerAuth = Settings.LOG_BEARERAUTH(_config);
    }

    public async Task Authenticate(TokenValidatedContext context)
    {
      // set roles based on email-->organisation-->org.PrimaryRoleId
      var bearerToken = ((FrameRequestHeaders)context.HttpContext.Request.Headers).HeaderAuthorization.Single();
      LogInformation($"Extracted token --> [{bearerToken}]");

      // have to cache responses or UserInfo endpoint thinks we are a DOS attack
      CachedUserInfoResponse cachedresponse = null;
      if (_cache.TryGetValue(bearerToken, out string jsonCachedResponse))
      {
        LogInformation($"cache[{bearerToken}] --> [{jsonCachedResponse}]");
        cachedresponse = JsonConvert.DeserializeObject<CachedUserInfoResponse>(jsonCachedResponse);
        if (cachedresponse.Created < DateTime.UtcNow.Subtract(Expiry))
        {
          LogInformation($"Removing expired cached token --> [{bearerToken}]");
          _cache.Remove(bearerToken);
          cachedresponse = null;
        }
      }

      var userInfo = Settings.OIDC_USERINFO_URL(_config);
      if (cachedresponse == null)
      {
        var response = await _userInfoClient.GetAsync(userInfo, bearerToken.Substring(7));
        if (response == null)
        {
          _logger.LogError($"No response from [{userInfo}]");
          return;
        }
        LogInformation($"Updating token --> [{bearerToken}]");
        _cache.SafeAdd(bearerToken, JsonConvert.SerializeObject(new CachedUserInfoResponse(response)));
        cachedresponse = new CachedUserInfoResponse(response);
      }

      if (cachedresponse.Claims == null)
      {
        _logger.LogError($"No claims from [{userInfo}]");
        return;
      }

      var userClaims = cachedresponse.Claims;
      var claims = new List<Claim>(userClaims.Select(x => new Claim(x.Type, x.Value)));
      var email = userClaims.SingleOrDefault(x => x.Type == "email")?.Value;
      if (!string.IsNullOrEmpty(email))
      {
        var contact = _contactsDatastore.ByEmail(email);
        if (contact == null)
        {
          _logger.LogError($"No contact for [{email}]");
          return;
        }

        var org = _organisationDatastore.ByContact(contact.Id);
        if (org == null)
        {
          _logger.LogError($"No organisation for [{contact.Id}]");
          return;
        }

        switch (org.PrimaryRoleId)
        {
          case PrimaryRole.ApplicationServiceProvider:
            claims.Add(new Claim(ClaimTypes.Role, Roles.Supplier));
            break;

          case PrimaryRole.GovernmentDepartment:
            claims.Add(new Claim(ClaimTypes.Role, Roles.Admin));
            claims.Add(new Claim(ClaimTypes.Role, Roles.Buyer));
            break;
        }
        claims.Add(new Claim(nameof(Organisations), org.Id));
      }

      context.Principal.AddIdentity(new ClaimsIdentity(claims));
    }

    private void LogInformation(string msg)
    {
      if (_logBearerAuth)
      {
        _logger.LogInformation(msg);
      }
    }
  }
#pragma warning restore CS1591
}

