using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Gif.Service
{
  public class AuthConfig
  {
    public static IEnumerable<Client> GetClients(IConfiguration config)
    {
      return new List<Client>
      {
        new Client
        {
          ClientId = Settings.CRM_CLIENTID(config),

          // no interactive user, use the clientid/secret for authentication
          AllowedGrantTypes = GrantTypes.ClientCredentials,

          // secret for authentication
          ClientSecrets =
          {
            new Secret(Settings.CRM_CLIENTSECRET(config).Sha256())
          },

          // scopes that client has access to
          AllowedScopes = { "GIFBuyingCatalogue" },
          AccessTokenLifetime = 7200 //2 hours
        }
      };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
      return new List<ApiResource>
      {
        new ApiResource("GIFBuyingCatalogue", "GIF Buying Catalogue")
      };
    }
  }
}
