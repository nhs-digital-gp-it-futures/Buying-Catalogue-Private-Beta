using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class CapabilitiesDatastore : DatastoreBase<Capabilities>, ICapabilitiesDatastore
  {
    public CapabilitiesDatastore(IDbConnectionFactory dbConnectionFactory, ILogger<CapabilitiesDatastore> logger, ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<Capabilities> ByFramework(string frameworkId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select cap.* from Capabilities cap
join CapabilityFramework cf on cf.CapabilityId = cap.Id
join Frameworks frame on frame.Id = cf.FrameworkId
where frame.Id = @frameworkId
";
        var retval = _dbConnection.Value.Query<Capabilities>(sql, new { frameworkId });
        return retval;
      });
    }

    public Capabilities ById(string id)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<Capabilities>(id);
      });
    }

    public IEnumerable<Capabilities> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select * from Capabilities
where Id in @ids";

        var retval = _dbConnection.Value.Query<Capabilities>(sql, new { ids });
        return retval;
      });
    }

    public IEnumerable<Capabilities> ByStandard(string standardId, bool isOptional)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select cap.* from Capabilities cap
join CapabilityStandard cs on cs.CapabilityId = cap.Id
join Standards std on std.Id = cs.StandardId
where std.Id = @standardId and cs.IsOptional = @isOptional
";
        var retval = _dbConnection.Value.Query<Capabilities>(sql, new { standardId, isOptional = (isOptional ? 1 : 0).ToString() });
        return retval;
      });
    }

    public IEnumerable<Capabilities> GetAll()
    {
      return GetInternal(() =>
      {
        const string sql = @"
-- select all current versions
select * from Capabilities where Id not in 
(
  select PreviousId from Capabilities where PreviousId is not null
)
";
        return _dbConnection.Value.Query<Capabilities>(sql);
      });
    }
  }
}
