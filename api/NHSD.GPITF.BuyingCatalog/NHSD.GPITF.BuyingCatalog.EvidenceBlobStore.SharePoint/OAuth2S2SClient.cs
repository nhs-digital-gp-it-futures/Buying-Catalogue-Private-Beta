using System;
using System.IO;
using System.Net;

namespace SharePointPnP.IdentityModel.Extensions.S2S.Protocols.OAuth2
{
  public sealed class OAuth2S2SClient
  {
    public OAuth2Message Issue(string securityTokenServiceUrl, OAuth2AccessTokenRequest oauth2Request)
    {
      OAuth2WebRequest oAuth2WebRequest = new OAuth2WebRequest(securityTokenServiceUrl, oauth2Request);
      OAuth2Message result;
      try
      {
        WebResponse response = oAuth2WebRequest.GetResponse();
        result = OAuth2MessageFactory.CreateFromEncodedResponse(new StreamReader(response.GetResponseStream()));
      }
      catch (Exception innerException)
      {
        throw new Exception("Token request failed.", innerException);
      }

      return result;
    }
  }
}
