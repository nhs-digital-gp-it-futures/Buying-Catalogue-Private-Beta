using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class FrameworksDatastore : DatastoreBase<Frameworks>, IFrameworksDatastore
  {
    public FrameworksDatastore(IDbConnectionFactory dbConnectionFactory, ILogger<FrameworksDatastore> logger, ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<Frameworks> ByCapability(string capabilityId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select frame.* from Frameworks frame
join CapabilityFramework cf on cf.FrameworkId = frame.Id
join Capabilities cap on cap.Id = cf.CapabilityId
where cap.Id = @capabilityId
";
        var retval = _dbConnection.Value.Query<Frameworks>(sql, new { capabilityId });
        return retval;
      });
    }

    public Frameworks ById(string id)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<Frameworks>(id);
      });
    }

    public IEnumerable<Frameworks> BySolution(string solutionId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select frame.* from Frameworks frame
join FrameworkSolution fs on fs.FrameworkId = frame.Id
join Solutions soln on soln.Id = fs.SolutionId
where soln.Id = @solutionId
";
        var retval = _dbConnection.Value.Query<Frameworks>(sql, new { solutionId });
        return retval;
      });
    }

    public IEnumerable<Frameworks> ByStandard(string standardId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select frame.* from Frameworks frame
join FrameworkStandard fs on fs.FrameworkId = frame.Id
join Standards std on std.Id = fs.StandardId
where std.Id = @standardId
";
        var retval = _dbConnection.Value.Query<Frameworks>(sql, new { standardId });
        return retval;
      });
    }

    public IEnumerable<Frameworks> GetAll()
    {
      return GetInternal(() =>
      {
        const string sql = @"
-- select all current versions
select * from Frameworks where Id not in 
(
  select PreviousId from Frameworks where PreviousId is not null
)
";
        return _dbConnection.Value.Query<Frameworks>(sql);
      });
    }
  }
}
