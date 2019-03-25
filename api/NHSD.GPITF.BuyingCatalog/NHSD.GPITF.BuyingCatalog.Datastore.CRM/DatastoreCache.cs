using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using Polly;
using StackExchange.Redis;
using System;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class DatastoreCache : IDatastoreCache
  {
    private readonly TimeSpan _expiry;
    private readonly IConfiguration _config;
    private readonly ILogger<DatastoreCache> _logger;
    private readonly ISyncPolicy _policy;
    private readonly ConnectionMultiplexer _redis;

    public DatastoreCache(
      IConfiguration config,
      ILogger<DatastoreCache> logger,
      ISyncPolicyFactory policy)
    {
      _config = config;
      _logger = logger;
      _policy = policy.Build(_logger);

      _expiry = TimeSpan.FromMinutes(Settings.CRM_CACHE_EXPIRY_MINS(_config));

      var cacheHost = Settings.CACHE_HOST(_config);
      var cfg = new ConfigurationOptions
      {
        EndPoints =
        {
          { cacheHost }
        },
        SyncTimeout = int.MaxValue
      };
      _redis = ConnectionMultiplexer.Connect(cfg);
    }

    public void SafeAdd(string path, string jsonCachedResponse)
    {
      GetInternal(() =>
      {
        _redis.GetDatabase().StringSet(path, jsonCachedResponse, _expiry);
        return 0;
      });
    }

    public bool TryGetValue(string path, out string jsonCachedResponse)
    {
      var cacheVal = GetInternal(() =>
      {
        return _redis.GetDatabase().StringGet(path);
      });
      jsonCachedResponse = cacheVal;
      return cacheVal.HasValue;
    }

    private TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }
  }
}
