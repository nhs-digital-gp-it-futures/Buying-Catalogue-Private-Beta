using System;
using System.Collections.Generic;

namespace SharePointPnP.IdentityModel.Extensions.S2S.Protocols.OAuth2
{
  public abstract class OAuth2Message
  {
    protected string this[string index]
    {
      get => GetValue(index);
      set => Message[index] = value;
    }

    protected IEnumerable<string> Keys => Message.Keys;

    public Dictionary<string, string> Message { get; } = new Dictionary<string, string>(StringComparer.Ordinal);

    public override string ToString() => Encode();

    protected bool ContainsKey(string key) => Message.ContainsKey(key);

    protected void Decode(string message) => Message.Decode(message);

    protected void DecodeFromJson(string message) => Message.DecodeFromJson(message);

    protected string Encode() => Message.Encode();

    protected string EncodeToJson() => Message.EncodeToJson();

    protected string GetValue(string key)
    {
      if (string.IsNullOrEmpty(key))
      {
        throw new ArgumentException("The input string parameter is either null or empty.", "key");
      }

      Message.TryGetValue(key, out string result);

      return result;
    }
  }
}
