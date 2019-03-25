using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Polly;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public abstract class DatastoreBase<T>
  {
    protected readonly IRestClientFactory _crmFactory;
    private readonly bool _logCRM;
    protected readonly ILogger<DatastoreBase<T>> _logger;
    private readonly ISyncPolicy _policy;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings();

    public DatastoreBase(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<T>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config)
    {
      _crmFactory = crmFactory;
      _logCRM = Settings.LOG_CRM(config);
      _logger = logger;
      _policy = policy.Build(_logger);

      _settings.Converters.Add(
        new StringEnumConverter
        {
          CamelCaseText = false
        });
    }

    protected TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }

    protected string GetLogMessage(IEnumerable<T> infos, [CallerMemberName] string caller = "")
    {
      return caller + " --> " + JArray.FromObject(infos).ToString(Formatting.None);
    }

    protected string GetLogMessage(Organisations organisation, [CallerMemberName] string caller = "")
    {
      return caller + " --> " + JObject.FromObject(organisation).ToString(Formatting.None);
    }

    protected string GetLogMessage(object info, [CallerMemberName] string caller = "")
    {
      return caller + " --> " + JObject.FromObject(info).ToString(Formatting.None);
    }

    protected RestRequest GetRequest(string path)
    {
      var request = new RestRequest(path)
      {
        Method = Method.GET
      };
      request.AddHeader("Content-Type", "application/json");
      request.AddHeader("Authorization", $"{_crmFactory.GetAccessToken()?.token_type} {_crmFactory.GetAccessToken()?.access_token}");

      return request;
    }

    protected RestRequest GetAllRequest(string path)
    {
      var request = GetRequest(path);
      AddGetAllParameters(request);

      return request;
    }

    protected RestRequest GetRequest(string path, object body)
    {
      var request = GetRequest(path);
      var serialiser = JsonSerializer.Create(_settings);
      request.JsonSerializer = new RestSharpJsonNetSerializer(serialiser);
      request.AddJsonBody(body);

      return request;
    }

    protected RestRequest GetPostRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = Method.POST;

      return request;
    }

    protected RestRequest GetAllPostRequest(string path, object body)
    {
      var request = GetPostRequest(path, body);
      AddGetAllParameters(request);

      return request;
    }

    protected RestRequest GetPutRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = Method.PUT;

      return request;
    }

    protected RestRequest GetDeleteRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = Method.DELETE;

      return request;
    }

    protected IRestResponse GetRawResponse(RestRequest request)
    {
      var resp = _crmFactory.GetRestClient().Execute(request);

      // log here as may fail deserialisation
      var body = request.Parameters.SingleOrDefault(x => x.Type == ParameterType.RequestBody)?.Value?.ToString();
      var msg = $"[{request.Resource}] {body} --> [{resp.StatusCode}] {resp.Content}";
      LogInformation(msg);

      // relog errors explicitly so they are more prominent
      if (resp.StatusCode >= HttpStatusCode.BadRequest)
      {
        _logger.LogError(msg);
      }

      return resp;
    }

    protected TOther GetResponse<TOther>(RestRequest request)
    {
      var resp = GetRawResponse(request);
      var retval = JsonConvert.DeserializeObject<TOther>(resp.Content, _settings);

      return retval;
    }

    private static void AddGetAllParameters(RestRequest request)
    {
      const int StartPageIndex = 1;
      const int GetAllPageSize = int.MaxValue;

      request.AddQueryParameter("PageIndex", StartPageIndex.ToString(CultureInfo.InvariantCulture));
      request.AddQueryParameter("PageSize", GetAllPageSize.ToString(CultureInfo.InvariantCulture));
    }

    protected static string UpdateId(string proposedId)
    {
      if (Guid.Empty.ToString() == proposedId)
      {
        return Guid.NewGuid().ToString();
      }

      if (string.IsNullOrWhiteSpace(proposedId))
      {
        return Guid.NewGuid().ToString();
      }

      return proposedId;
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
