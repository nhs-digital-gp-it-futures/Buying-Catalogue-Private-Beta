using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace SharePointPnP.IdentityModel.Extensions.S2S.Protocols.OAuth2
{
  public sealed class OAuth2AccessTokenRequest : OAuth2Message
  {
    public static StringCollection TokenResponseParameters = GetTokenResponseParameters();

    public string Password
    {
      get => Message["password"];
      set => Message["password"] = value;
    }

    public string RefreshToken
    {
      get => Message["refresh_token"];
      set => Message["refresh_token"] = value;
    }

    public string Resource
    {
      get => Message["resource"];
      set => Message["resource"] = value;
    }

    public string Scope
    {
      get => Message["scope"];
      set => Message["scope"] = value;
    }

    public string AppContext
    {
      get => base["AppContext"];
      set => base["AppContext"] = value;
    }

    public string Assertion
    {
      get => base["assertion"];
      set => base["assertion"] = value;
    }

    public string GrantType
    {
      get => base["grant_type"];
      set => base["grant_type"] = value;
    }

    public string ClientId
    {
      get => base["client_id"];
      set => base["client_id"] = value;
    }

    public string ClientSecret
    {
      get => base["client_secret"];
      set => base["client_secret"] = value;
    }

    public string Code
    {
      get => base["code"];
      set => base["code"] = value;
    }

    public string RedirectUri
    {
      get => base["redirect_uri"];
      set => base["redirect_uri"] = value;
    }

    public static OAuth2AccessTokenRequest Read(StreamReader reader)
    {
      string requestString = null;
      try
      {
        requestString = reader.ReadToEnd();
      }
      catch (DecoderFallbackException innerException)
      {
        throw new InvalidDataException("Request encoding is not ASCII", innerException);
      }

      return Read(requestString);
    }

    public static OAuth2AccessTokenRequest Read(string requestString)
    {
      var oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest();
      try
      {
        oAuth2AccessTokenRequest.Decode(requestString);
      }
      catch (Exception)
      {
        NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(requestString);
        if (string.IsNullOrEmpty(nameValueCollection["client_id"]) && string.IsNullOrEmpty(nameValueCollection["assertion"]))
        {
          throw new InvalidDataException("The request body must contain a client_id or assertion parameter.");
        }

        throw;
      }

      foreach (string current in oAuth2AccessTokenRequest.Keys)
      {
        if (TokenResponseParameters.Contains(current))
        {
          throw new InvalidDataException();
        }
      }

      return oAuth2AccessTokenRequest;
    }

    private static StringCollection GetTokenResponseParameters() => new StringCollection { "access_token", "expires_in" };

    public void SetCustomProperty(string propertyName, string propertyValue)
    {
      propertyName.ValidateNotNullOrEmpty("propertyName");
      propertyValue.ValidateNotNullOrEmpty("propertyValue");
      base[propertyName] = propertyValue;
    }

    public void Write(StreamWriter writer)
    {
      if (writer == null)
      {
        throw new ArgumentNullException("writer");
      }

      writer.Write(Encode());
    }
  }
}
