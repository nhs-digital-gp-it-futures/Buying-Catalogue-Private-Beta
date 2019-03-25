using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class StandardsDatastore : DatastoreBase<Standards>, IStandardsDatastore
  {
    public StandardsDatastore(IDbConnectionFactory dbConnectionFactory, ILogger<StandardsDatastore> logger, ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<Standards> ByCapability(string capabilityId, bool isOptional)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select std.* from Standards std
join CapabilityStandard cs on cs.StandardId = std.Id
join Capabilities cap on cap.Id = cs.CapabilityId
where cap.Id = @capabilityId and cs.IsOptional = @isOptional
";
        var retval = _dbConnection.Value.Query<Standards>(sql, new { capabilityId, isOptional = (isOptional ? 1 : 0).ToString()});
        return retval;
      });
    }

    public IEnumerable<Standards> ByFramework(string frameworkId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select std.* from Standards std
join FrameworkStandard fs on fs.StandardId = std.Id
join Frameworks frame on frame.Id = fs.FrameworkId
where frame.Id = @frameworkId
";
        var retval = _dbConnection.Value.Query<Standards>(sql, new { frameworkId });
        return retval;
      });
    }

    public Standards ById(string id)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<Standards>(id);
      });
    }

    public IEnumerable<Standards> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select * from Standards
where Id in @ids
";
        var retval = _dbConnection.Value.Query<Standards>(sql, new { ids });
        return retval;
      });
    }

    public Standards Create(Standards standard)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          standard.Id = UpdateId(standard.Id);
          _dbConnection.Value.Insert(standard, trans);
          trans.Commit();

          return standard;
        }
      });
    }

    public IEnumerable<Standards> GetAll()
    {
      return GetInternal(() =>
      {
        const string sql = @"
-- select all current versions
select * from Standards where Id not in 
(
  select PreviousId from Standards where PreviousId is not null
)
";
        return _dbConnection.Value.Query<Standards>(sql);
      });
    }

    public void Update(Standards standard)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          _dbConnection.Value.Update(standard, trans);
          trans.Commit();
          return 0;
        }
      });
    }
  }
}
