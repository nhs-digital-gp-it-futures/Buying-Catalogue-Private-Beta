using System;

namespace SharePointPnP.IdentityModel.Extensions.S2S.Protocols.OAuth2
{
  public sealed class OAuth2ErrorResponse : OAuth2Message
  {
    public string Error
    {
      get => Message["error"];
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          throw new ArgumentException("Error property cannot be null or empty.", "value");
        }
        Message["error"] = value;
      }
    }

    public string ErrorDescription
    {
      get => Message["error_description"];
      set => Message["error_description"] = value;
    }

    public string ErrorUri
    {
      get => Message["error_uri"];
      set => Message["error_uri"] = value;
    }

    public static OAuth2ErrorResponse CreateFromEncodedResponse(string responseString)
    {
      OAuth2ErrorResponse oAuth2ErrorResponse = new OAuth2ErrorResponse();
      oAuth2ErrorResponse.DecodeFromJson(responseString);
      if (string.IsNullOrEmpty(oAuth2ErrorResponse.Error))
      {
        throw new ArgumentException("Error property is null or empty. This message is not a valid OAuth2 error response.", "responseString");
      }

      return oAuth2ErrorResponse;
    }

    private OAuth2ErrorResponse()
    {
    }

    public OAuth2ErrorResponse(string error) => Error = error;

    public override string ToString() => EncodeToJson();
  }
}
