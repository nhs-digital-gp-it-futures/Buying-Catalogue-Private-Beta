using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SharePointPnP.IdentityModel.Extensions.S2S
{
  public static class DictionaryExtension
  {
    public delegate string Encoder(string input);

    public const char DefaultSeparator = '&';

    public const char DefaultKeyValueSeparator = '=';

    public static Encoder DefaultDecoder = new Encoder(HttpUtility.UrlDecode);

    public static Encoder DefaultEncoder = new Encoder(HttpUtility.UrlEncode);

    public static Encoder NullEncoder = new Encoder(NullEncode);

    private static string NullEncode(string value) => value;

    public static void Decode(this IDictionary<string, string> self, string encodedDictionary) => self.Decode(encodedDictionary, '&', '=', DefaultDecoder, DefaultDecoder, false);

    public static void Decode(this IDictionary<string, string> self, string encodedDictionary, Encoder decoder) => self.Decode(encodedDictionary, '&', '=', decoder, decoder, false);

    public static void Decode(this IDictionary<string, string> self, string encodedDictionary, char separator, char keyValueSplitter, bool endsWithSeparator) => self.Decode(encodedDictionary, separator, keyValueSplitter, DefaultDecoder, DefaultDecoder, endsWithSeparator);

    public static void Decode(this IDictionary<string, string> self, string encodedDictionary, char separator, char keyValueSplitter, Encoder keyDecoder, Encoder valueDecoder, bool endsWithSeparator)
    {
      if (encodedDictionary == null)
      {
        throw new ArgumentNullException("encodedDictionary");
      }

      if (keyDecoder == null)
      {
        throw new ArgumentNullException("keyDecoder");
      }

      if (valueDecoder == null)
      {
        throw new ArgumentNullException("valueDecoder");
      }

      if (endsWithSeparator && encodedDictionary.LastIndexOf(separator) == encodedDictionary.Length - 1)
      {
        encodedDictionary = encodedDictionary.Substring(0, encodedDictionary.Length - 1);
      }

      string[] array = encodedDictionary.Split(new char[] { separator });

      for (int i = 0; i < array.Length; i++)
      {
        string text = array[i];
        string[] array2 = text.Split(new char[] { keyValueSplitter });
        if ((array2.Length == 1 || array2.Length > 2) && !string.IsNullOrEmpty(array2[0]))
        {
          throw new ArgumentException("The request is not properly formatted.", "encodedDictionary");
        }

        if (array2.Length != 2)
        {
          throw new ArgumentException("The request is not properly formatted.", "encodedDictionary");
        }
        string text2 = keyDecoder(array2[0].Trim());
        string value = valueDecoder(array2[1].Trim().Trim(new char[] { '"' }));

        try
        {
          self.Add(text2, value);
        }
        catch (ArgumentException)
        {
          string message = string.Format(System.Globalization.CultureInfo.InvariantCulture, "The request is not properly formatted. The parameter '{0}' is duplicated.", new object[] { text2 });
          throw new ArgumentException(message, "encodedDictionary");
        }
      }
    }

    public static string Encode(this IDictionary<string, string> self) => self.Encode('&', '=', DefaultEncoder, DefaultEncoder, false);

    public static string Encode(this IDictionary<string, string> self, Encoder encoder) => self.Encode('&', '=', encoder, encoder, false);

    public static string Encode(this IDictionary<string, string> self, char separator, char keyValueSplitter, bool endsWithSeparator) => self.Encode(separator, keyValueSplitter, DefaultEncoder, DefaultEncoder, endsWithSeparator);

    public static string Encode(this IDictionary<string, string> self, char separator, char keyValueSplitter, Encoder keyEncoder, Encoder valueEncoder, bool endsWithSeparator)
    {
      if (keyEncoder == null)
      {
        throw new ArgumentNullException("keyEncoder");
      }

      if (valueEncoder == null)
      {
        throw new ArgumentNullException("valueEncoder");
      }

      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> current in self)
      {
        if (stringBuilder.Length != 0)
        {
          stringBuilder.Append(separator);
        }

        stringBuilder.AppendFormat("{0}{1}{2}", keyEncoder(current.Key), keyValueSplitter, valueEncoder(current.Value));
      }

      if (endsWithSeparator)
      {
        stringBuilder.Append(separator);
      }

      return stringBuilder.ToString();
    }

    public static string EncodeToJson(this IDictionary<string, string> self) => JsonConvert.SerializeObject(self);

    public static void DecodeFromJson(this IDictionary<string, string> self, string encodedDictionary)
    {
      var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(encodedDictionary);
      if (dictionary == null)
      {
        throw new ArgumentException("Invalid request format.", "encodedDictionary");
      }

      foreach (KeyValuePair<string, object> current in dictionary)
      {
        if (current.Value == null)
        {
          self.Add(current.Key, null);
        }
        else if (current.Value is object[])
        {
          self.Add(current.Key, JsonConvert.SerializeObject(current.Value));
        }
        else
        {
          self.Add(current.Key, current.Value.ToString());
        }
      }
    }
  }
}
