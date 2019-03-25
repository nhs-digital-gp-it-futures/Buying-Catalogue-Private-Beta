#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Enums;
using Gif.Service.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Samc4.CipherUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Gif.Service.Crm
{
  public sealed class Repository : IRepository
  {
    private readonly IConfiguration _config;
    private readonly ILogger<Repository> _logger;
    private readonly bool _logCRM;
    private HttpClient _httpClient;
    private AuthenticationContext _authContext;
    private AccessToken _cachedAccessToken;

    public Repository(
      IConfiguration config,
      ILogger<Repository> logger)
    {
      _config = config;
      _logger = logger;
      _logCRM = Settings.LOG_CRM(config);

      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      CreateCrmConnection();
    }

    private void CreateCrmConnection()
    {
      var httpHandler = new HttpClientHandler { Proxy = null, UseProxy = false };

      _httpClient = new HttpClient(httpHandler)
      {
        BaseAddress = new Uri(Settings.GIF_CRM_URL(_config) + "/api/data/v9.0/"),
        Timeout = new TimeSpan(0, 2, 0)
      };
      _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
      _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
      _httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=OData.Community.Display.V1.FormattedValue");
      _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      _authContext = new AuthenticationContext(Settings.GIF_CRM_AUTHORITY(_config), false);
      ApplyAccessToken();
    }

    private void ApplyAccessToken()
    {
      if (_cachedAccessToken == null)
        _cachedAccessToken = CreateAccessToken();

      if (DateTime.UtcNow >= _cachedAccessToken.expires_on)
        _cachedAccessToken = CreateAccessToken();

      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cachedAccessToken.access_token);
    }

    private AccessToken CreateAccessToken()
    {
      var secret = CipherUtil.Decrypt<AesManaged>(Settings.GIF_ENCRYPTED_CLIENT_SECRET(_config), "GifService", Settings.GIF_AZURE_CLIENT_ID(_config));

      var result = _authContext
          .AcquireTokenAsync(Settings.GIF_CRM_URL(_config), new ClientCredential(Settings.GIF_AZURE_CLIENT_ID(_config), secret))
          .Result;

      var accessToken = new AccessToken
      {
        expires_on = result.ExpiresOn,
        access_token = result.AccessToken
      };

      return accessToken;
    }

    public JToken RetrieveMultiple(string query, out int? count)
    {
      LogInformation($"[{nameof(RetrieveMultiple)}] --> {query}");
      ApplyAccessToken();

      JToken jretrieveToken = null;
      count = null;

      HttpResponseMessage retrieveResponse;
      retrieveResponse = _httpClient.GetAsync(query).Result;

      if (retrieveResponse.StatusCode != HttpStatusCode.OK && retrieveResponse.StatusCode != HttpStatusCode.NoContent)
      {
        var ex = new CrmApiException(retrieveResponse.ReasonPhrase, retrieveResponse.StatusCode);
        _logger.LogError(ex, $"[{ex.HttpStatus}] + [{ex.Message}]");
        throw ex;
      }

      var jretrieveJObject = JObject.Parse(retrieveResponse.Content.ReadAsStringAsync().Result);

      if (jretrieveJObject == null)
      {
        return jretrieveToken;
      }

      jretrieveToken = jretrieveJObject["value"];

      if (jretrieveJObject["@odata.count"] != null)
      {
        count = int.Parse(jretrieveJObject["@odata.count"].ToString());
      }

      LogInformation($"  [{retrieveResponse.StatusCode}] --> {jretrieveToken.ToString()}");
      return jretrieveToken;
    }

    public void Associate(Guid entityId1, string entityName1, Guid entityId2, string entityName2, string relationshipKey)
    {
      LogInformation($"[{nameof(Associate)}] --> {entityName1}/{entityId1} -> [{relationshipKey}] -> {entityName2}/{entityId2}");
      ApplyAccessToken();

      HttpResponseMessage resp;

      var address = $"{entityName1}({entityId1.ToString()})/{relationshipKey}/$ref";
      var associated = $"{{\"@odata.id\":\"{_httpClient.BaseAddress}{entityName2}({entityId2})\"}}";
      var content = new StringContent(associated, Encoding.UTF8, "application/json");

      resp = _httpClient.PostAsync(address, content).Result;

      if (resp.StatusCode != HttpStatusCode.OK && resp.StatusCode != HttpStatusCode.NoContent)
      {
        var ex = new CrmApiException(resp.ReasonPhrase, resp.StatusCode);
        _logger.LogError(ex, $"[{ex.HttpStatus}] + [{ex.Message}]");
        throw ex;
      }
    }

    public Guid CreateEntity(string entityName, string entityData, bool update = false)
    {
      LogInformation($"[{nameof(CreateEntity)}] --> {entityName}/{entityData}");
      ApplyAccessToken();

      var address = entityName;
      var content = new StringContent(entityData, Encoding.UTF8, "application/json");

      HttpResponseMessage updateResponse;
      string targetUri;

      updateResponse = _httpClient.PostAsync(address, content).Result;
      targetUri = _httpClient.BaseAddress.AbsoluteUri;


      if (updateResponse.StatusCode != HttpStatusCode.OK && updateResponse.StatusCode != HttpStatusCode.NoContent)
      {
        var ex = new CrmApiException(updateResponse.ReasonPhrase, updateResponse.StatusCode);
        _logger.LogError(ex, $"[{ex.HttpStatus}] + [{ex.Message}]");
        throw ex;
      }

      IEnumerable<string> headerVals;

      if (!updateResponse.Headers.TryGetValues("OData-EntityId", out headerVals))
      {
        var ex = new FormatException("Response Entity ID header is empty");
        _logger.LogError(ex, $"[FormatException] + [{ex.Message}]");
        throw ex;
      }

      var idString = new List<string>(headerVals)[0].Replace(targetUri + entityName, "");

      return Guid.Parse(idString);
    }

    public void UpdateEntity(string entityName, Guid id, string entityData)
    {
      LogInformation($"[{nameof(UpdateEntity)}] --> {entityName}/{entityData}");
      ApplyAccessToken();

      HttpResponseMessage response;
      var method = new HttpMethod("PATCH");
      var content = new StringContent(entityData, Encoding.UTF8, "application/json");


      var requestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}{entityName}({id})");

      var request = new HttpRequestMessage(method, requestUri)
      {
        Content = content
      };

      response = _httpClient.SendAsync(request).Result;


      if (response.StatusCode != HttpStatusCode.NoContent)
      {
        var ex = new CrmApiException(response.ReasonPhrase, response.StatusCode);
        _logger.LogError(ex, $"[{ex.HttpStatus}] + [{ex.Message}]");
        throw ex;
      }
    }

    public void Delete(string entityName, Guid id)
    {
      LogInformation($"[{nameof(Delete)}] --> {entityName}/{id}");
      ApplyAccessToken();

      HttpResponseMessage response;
      var method = new HttpMethod("DELETE");


      var requestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}{entityName}({id})");

      var request = new HttpRequestMessage(method, requestUri);
      response = _httpClient.SendAsync(request).Result;

      if (response.StatusCode != HttpStatusCode.NoContent)
      {
        var ex = new CrmApiException(response.ReasonPhrase, response.StatusCode);
        _logger.LogError(ex, $"[{ex.HttpStatus}] + [{ex.Message}]");
        throw ex;
      }
    }

    public void CreateBatch(List<BatchData> batchData)
    {
      LogInformation($"[{nameof(CreateBatch)}] --> {JsonConvert.SerializeObject(batchData)}");
      ApplyAccessToken();

      HttpResponseMessage response;

      // batch setup
      var batchId = Guid.NewGuid();
      var deleteChangeId = Guid.NewGuid();
      var patchChangeId = Guid.NewGuid();
      var batchUrl = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}$batch");

      var batchRequest = new HttpRequestMessage(HttpMethod.Post, batchUrl) { Version = new Version(1, 1) };
      var batchContent = new MultipartContent("mixed", "batch_" + batchId);

      // changeset setup
      var deleteChange = new MultipartContent("mixed", "changeset_" + deleteChangeId);
      var patchChange = new MultipartContent("mixed", "changeset_" + patchChangeId);

      //Add deletes first in their own changeset to avoid issues with upserts in following changesets
      AddChangeSet(batchData.Where(x => x.Type == BatchTypeEnum.Delete).ToList(), _httpClient, ref deleteChange);
      AddChangeSet(batchData.Where(x => x.Type != BatchTypeEnum.Delete).ToList(), _httpClient, ref patchChange);

      //Add headers add changeset level
      AddHeadersToChangeSets(ref deleteChange);
      AddHeadersToChangeSets(ref patchChange);

      // Add the changesets to the batch content
      batchContent.Add(deleteChange);
      batchContent.Add(patchChange);

      // send batch
      batchRequest.Content = batchContent;
      response = _httpClient.SendAsync(batchRequest).Result;

      if (response.StatusCode != HttpStatusCode.OK)
      {
        var ex = new CrmApiException(response.ReasonPhrase, response.StatusCode);
        _logger.LogError(ex, $"[{ex.HttpStatus}] + [{ex.Message}]");
        throw ex;
      }
    }

    private void AddChangeSet(IList<BatchData> batchData, HttpClient httpClient, ref MultipartContent changeSet)
    {
      var count = 1;

      foreach (var batch in batchData)
      {
        var content = new StringContent(batch.EntityData, Encoding.UTF8, "application/json");
        var requestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{batch.Name}({batch.Id})");
        content.Headers.Remove("Content-Type");
        content.Headers.Add("Content-Type", "application/json;type=entry");
        content.Headers.Add("Content-Transfer-Encoding", "binary");
        content.Headers.Add("Content-Id", count.ToString());

        var method = batch.Type == BatchTypeEnum.Delete ?
            new HttpMethod("DELETE") : new HttpMethod("PATCH");

        var request = new HttpRequestMessage(method, requestUri)
        {
          Content = content,
          Version = new Version(1, 1)
        };

        // Add this content to the changeset
        changeSet.Add(new HttpMessageContent(request));
        count++;
      }
    }

    private void AddHeadersToChangeSets(ref MultipartContent content)
    {
      using (var enumChangeSet = content.GetEnumerator())
      {
        while (enumChangeSet.MoveNext())
        {
          var currentChangeSet = enumChangeSet.Current;
          currentChangeSet.Headers.ContentType = new MediaTypeHeaderValue("application/http");
          currentChangeSet.Headers.Add("Content-Transfer-Encoding", "binary");
        }
      }
    }

    private void UpdateField(string entityName, string entityField, Guid entityId, string value)
    {
      var address = entityName + "(" + entityId + ")/" + entityField;

      //Don't add quotes if value is a bool/int
      bool boolField; int intField; string updateBody;
      if (bool.TryParse(value, out boolField) || int.TryParse(value, out intField))
      {
        updateBody = "{\"value\": " + value.ToLower() + "}";
      }
      else
      {
        updateBody = "{\"value\":\"" + value.ToLower() + "\"}";
      }

      var content = new StringContent(updateBody, Encoding.UTF8, "application/json");

      HttpResponseMessage updateResponse;

      updateResponse = _httpClient.PutAsync(address, content).Result;

      if (updateResponse.StatusCode != HttpStatusCode.NoContent)
      {
        var ex = new CrmApiException(updateResponse.ReasonPhrase, updateResponse.StatusCode);
        _logger.LogError(ex, $"[{ex.HttpStatus}] + [{ex.Message}]");
        throw ex;
      }
    }

    private void LogInformation(string msg)
    {
      if (_logCRM)
      {
        _logger.LogInformation(msg);
      }
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
