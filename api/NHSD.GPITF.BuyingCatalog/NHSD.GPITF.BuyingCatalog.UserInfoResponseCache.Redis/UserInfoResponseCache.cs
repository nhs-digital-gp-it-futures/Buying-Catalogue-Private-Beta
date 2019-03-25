using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using Polly;
using StackExchange.Redis;
using System;

namespace NHSD.GPITF.BuyingCatalog.UserInfoResponseCache.Redis
{
  public sealed class UserInfoResponseCache : IUserInfoResponseCache
  {
    // Actual expiry of token from Auth0 is 1 hour.
    // Expiry/removal from cache is managed by caller.
    // This expiry is provided so that cache does not fill up with expired tokens
    // which have not been removed by the caller.
    private static TimeSpan Expiry = TimeSpan.FromHours(2);

    private readonly IConfiguration _config;
    private readonly ILogger<UserInfoResponseCache> _logger;
    private readonly ISyncPolicy _policy;
    private readonly ConnectionMultiplexer _redis;

    public UserInfoResponseCache(
      IConfiguration config,
      ILogger<UserInfoResponseCache> logger,
      ISyncPolicyFactory policy)
    {
      _config = config;
      _logger = logger;
      _policy = policy.Build(_logger);

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

    public void Remove(string bearerToken)
    {
      GetInternal(() =>
      {
        _redis.GetDatabase().KeyDelete(bearerToken);
        return 0;
      });
    }

    public void SafeAdd(string bearerToken, string jsonCachedResponse)
    {
      GetInternal(() =>
      {
        _redis.GetDatabase().StringSet(bearerToken, jsonCachedResponse, Expiry);
        return 0;
      });
    }

    public bool TryGetValue(string bearerToken, out string jsonCachedResponse)
    {
      var cacheVal = GetInternal(() =>
      {
        return _redis.GetDatabase().StringGet(bearerToken);
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
