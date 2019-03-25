using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  // This class is used to get MetaData document from the global STS endpoint. It contains
  // methods to parse the MetaData document and get endpoints and STS certificate.
  public static class AcsMetadataParser
  {
    private const string S2SProtocol = "OAuth2";
    private const string AcsMetadataEndPointRelativeUrl = "metadata/json/1";

    private static string acsHostUrl = "accesscontrol.windows.net";
    private static string AcsHostUrl
    {
      get
      {
        if (String.IsNullOrEmpty(acsHostUrl))
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
    private static string GlobalEndPointPrefix
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

    private static JsonMetadataDocument GetMetadataDocument(string realm)
    {
      string acsMetadataEndpointUrlWithRealm = string.Format(CultureInfo.InvariantCulture, "{0}?realm={1}",
                                                             GetAcsMetadataEndpointUrl(),
                                                             realm);
      byte[] acsMetadata;
      using (WebClient webClient = new WebClient())
      {
        acsMetadata = webClient.DownloadData(acsMetadataEndpointUrlWithRealm);
      }
      string jsonResponseString = Encoding.UTF8.GetString(acsMetadata);

      JsonMetadataDocument document = JsonConvert.DeserializeObject<JsonMetadataDocument>(jsonResponseString);

      if (null == document)
      {
        throw new Exception("No metadata document found at the global endpoint " + acsMetadataEndpointUrlWithRealm);
      }

      return document;
    }

    private static string GetAcsMetadataEndpointUrl()
    {
      return Path.Combine(GetAcsGlobalEndpointUrl(), AcsMetadataEndPointRelativeUrl);
    }

    private static string GetAcsGlobalEndpointUrl()
    {
      if (GlobalEndPointPrefix.Length == 0)
      {
        return string.Format(CultureInfo.InvariantCulture, "https://{0}/", AcsHostUrl);
      }
      else
      {
        return string.Format(CultureInfo.InvariantCulture, "https://{0}.{1}/", GlobalEndPointPrefix, AcsHostUrl);
      }
    }

    public static string GetStsUrl(string realm)
    {
      JsonMetadataDocument document = GetMetadataDocument(realm);

      JsonEndpoint s2sEndpoint = document.Endpoints.SingleOrDefault(e => e.Protocol == S2SProtocol);

      if (null != s2sEndpoint)
      {
        return s2sEndpoint.Location;
      }

      throw new Exception("Metadata document does not contain STS endpoint URL");
    }

    #region private classes
    private class JsonMetadataDocument
    {
      public string ServiceName { get; set; }
      public List<JsonEndpoint> Endpoints { get; set; }
      public List<JsonKey> Keys { get; set; }
    }

    private class JsonEndpoint
    {
      public string Location { get; set; }
      public string Protocol { get; set; }
      public string Usage { get; set; }
    }

    private class JsonKeyValue
    {
      public string Type { get; set; }
      public string Value { get; set; }
    }

    private class JsonKey
    {
      public string Usage { get; set; }
      public JsonKeyValue KeyValue { get; set; }
    }
    #endregion
  }
}
