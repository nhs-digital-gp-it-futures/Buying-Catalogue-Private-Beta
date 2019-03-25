using Dapper;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class KeywordSearchHistoryDatastore : DatastoreBase<KeywordCount>, IKeywordSearchHistoryDatastore
  {
    public KeywordSearchHistoryDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<DatastoreBase<KeywordCount>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<KeywordCount> Get(DateTime startDate, DateTime endDate)
    {
      return GetInternal(() =>
      {
        const string SearchByKeywordCallsite = "NHSD.GPITF.BuyingCatalog.Search.Porcelain.SearchDatastore.ByKeyword";
        var sql = $@"
select log.Timestamp, log.Message as Keyword from Log log
where Callsite like '{SearchByKeywordCallsite}'
";
        // cannot use GetAll() because this requires an explicit key
        var searchHists = _dbConnection.Value.Query<KeywordSearchHistory>(sql)
          .Where(x => x.Timestamp >= startDate && x.Timestamp <= endDate);

        var retval = from hist in searchHists
                    group hist by hist.Keyword into buckets
                    select new KeywordCount { Keyword = buckets.Key, Count = buckets.Count() };

        return retval;
      });
    }
  }
}
