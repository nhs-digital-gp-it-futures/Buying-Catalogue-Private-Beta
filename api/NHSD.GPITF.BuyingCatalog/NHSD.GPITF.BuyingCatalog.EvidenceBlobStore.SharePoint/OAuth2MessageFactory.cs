namespace SharePointPnP.IdentityModel.Extensions.S2S.Protocols.OAuth2
{
  public static class OAuth2MessageFactory
  {
    public static OAuth2Message CreateFromEncodedResponse(System.IO.StreamReader reader)
    {
      return CreateFromEncodedResponse(reader.ReadToEnd());
    }

    public static OAuth2Message CreateFromEncodedResponse(string responseString)
    {
      if (responseString.StartsWith("{\"error"))
      {
        return OAuth2ErrorResponse.CreateFromEncodedResponse(responseString);
      }

      return OAuth2AccessTokenResponse.Read(responseString);
    }

    public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAuthorizationCode(string clientId, string clientSecret, string authorizationCode, System.Uri redirectUri, string resource)
    {
      OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest
      {
        GrantType = "authorization_code",
        ClientId = clientId,
        ClientSecret = clientSecret,
        Code = authorizationCode
      };

      if (redirectUri != null)
      {
        oAuth2AccessTokenRequest.RedirectUri = redirectUri.AbsoluteUri;
      }
      oAuth2AccessTokenRequest.Resource = resource;

      return oAuth2AccessTokenRequest;
    }

    public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAuthorizationCode(string clientId, string clientSecret, string authorizationCode, string resource)
    {
      return new OAuth2AccessTokenRequest
      {
        GrantType = "authorization_code",
        ClientId = clientId,
        ClientSecret = clientSecret,
        Code = authorizationCode,
        Resource = resource
      };
    }

    public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithRefreshToken(string clientId, string clientSecret, string refreshToken, string resource)
    {
      return new OAuth2AccessTokenRequest
      {
        GrantType = "refresh_token",
        ClientId = clientId,
        ClientSecret = clientSecret,
        RefreshToken = refreshToken,
        Resource = resource
      };
    }

    public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithClientCredentials(string clientId, string clientSecret, string scope)
    {
      return new OAuth2AccessTokenRequest
      {
        GrantType = "client_credentials",
        ClientId = clientId,
        ClientSecret = clientSecret,
        Scope = scope
      };
    }
  }
}
