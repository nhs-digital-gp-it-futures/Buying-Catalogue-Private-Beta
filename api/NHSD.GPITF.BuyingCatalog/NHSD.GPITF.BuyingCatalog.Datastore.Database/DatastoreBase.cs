using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public abstract class DatastoreBase<T>
  {
    protected readonly Lazy<IDbConnection> _dbConnection;
    protected readonly ILogger<DatastoreBase<T>> _logger;
    private readonly ISyncPolicy _policy;

    public DatastoreBase(IDbConnectionFactory dbConnectionFactory, ILogger<DatastoreBase<T>> logger, ISyncPolicyFactory policy)
    {
      _dbConnection = new Lazy<IDbConnection>(() => dbConnectionFactory.Get());
      _logger = logger;
      _policy = policy.Build(_logger);
    }

    protected string UpdateId(string proposedId)
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
  }
}
