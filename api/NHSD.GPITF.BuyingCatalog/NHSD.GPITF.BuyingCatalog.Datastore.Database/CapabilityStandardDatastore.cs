using Dapper;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class CapabilityStandardDatastore : DatastoreBase<CapabilityStandard>, ICapabilityStandardDatastore
  {
    public CapabilityStandardDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<CapabilityStandardDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<CapabilityStandard> GetAll()
    {
      return GetInternal(() =>
      {
        // cannot use GetAll() as Dapper does not support composite primary keys
        const string sql = @"
select cs.* from CapabilityStandard cs
";
        var retval = _dbConnection.Value.Query<CapabilityStandard>(sql);
        return retval;
      });
    }
  }
}
