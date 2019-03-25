using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public abstract class CachedDatastore<T> : DatastoreBase<T>
  {
    private readonly bool _logCRM;
    protected readonly IDatastoreCache _cache;

    public CachedDatastore(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<T>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config,
      IDatastoreCache cache) :
      base(crmFactory, logger, policy, config)
    {
      _logCRM = Settings.LOG_CRM(config);
      _cache = cache;
    }

    protected T Get(string path)
    {
      LogInformation($"[{path}]");
      if (_cache.TryGetValue(path, out string jsonCachedResponse))
      {
        LogInformation($"cache[{path}] --> [{jsonCachedResponse}]");
        return JsonConvert.DeserializeObject<T>(jsonCachedResponse);
      }

      var request = GetRequest(path);
      var retval = GetResponse<T>(request);

      _cache.SafeAdd(path, JsonConvert.SerializeObject(retval));

      return retval;
    }

    protected IEnumerable<T> GetAll(string path)
    {
      LogInformation($"[{path}]");
      if (_cache.TryGetValue(path, out string jsonCachedResponse))
      {
        LogInformation($"cache[{path}] --> [{jsonCachedResponse}]");
        return JsonConvert.DeserializeObject<PaginatedList<T>>(jsonCachedResponse).Items;
      }

      var request = GetAllRequest(path);
      var retval = GetResponse<PaginatedList<T>>(request);

      _cache.SafeAdd(path, JsonConvert.SerializeObject(retval));

      return retval.Items;
    }

    protected void LogInformation(string msg)
    {
      if (_logCRM)
      {
        _logger.LogInformation(msg);
      }
    }
  }
}
