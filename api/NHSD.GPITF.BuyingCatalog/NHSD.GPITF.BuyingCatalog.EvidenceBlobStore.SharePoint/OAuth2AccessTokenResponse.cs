using System;

namespace SharePointPnP.IdentityModel.Extensions.S2S.Protocols.OAuth2
{
  public sealed class OAuth2AccessTokenResponse : OAuth2Message
  {
    public string AccessToken
    {
      get => Message["access_token"];
      set => Message["access_token"] = value;
    }

    public string ExpiresIn
    {
      get => Message["expires_in"];
      set => Message["expires_in"] = value;
    }

    public DateTime ExpiresOn
    {
      get => GetDateTimeParameter("expires_on");
      set => SetDateTimeParameter("expires_on", value);
    }

    public DateTime NotBefore
    {
      get => GetDateTimeParameter("not_before");
      set => SetDateTimeParameter("not_before", value);
    }

    public string RefreshToken
    {
      get => Message["refresh_token"];
      set => Message["refresh_token"] = value;
    }

    public string Scope
    {
      get => Message["scope"];
      set => Message["scope"] = value;
    }

    public string TokenType
    {
      get => Message["token_type"];
      set => Message["token_type"] = value;
    }

    public static OAuth2AccessTokenResponse Read(string responseString)
    {
      OAuth2AccessTokenResponse oAuth2AccessTokenResponse = new OAuth2AccessTokenResponse();
      oAuth2AccessTokenResponse.DecodeFromJson(responseString);

      return oAuth2AccessTokenResponse;
    }

    public override string ToString() => EncodeToJson();

    private DateTime GetDateTimeParameter(string parameterName) => new EpochTime(base.Message[parameterName]).DateTime;

    private void SetDateTimeParameter(string parameterName, DateTime value) => Message[parameterName] = new EpochTime(value).SecondsSinceUnixEpoch.ToString();
  }
}
