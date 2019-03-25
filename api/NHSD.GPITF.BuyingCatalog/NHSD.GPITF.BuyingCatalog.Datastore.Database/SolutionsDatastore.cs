using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class SolutionsDatastore : DatastoreBase<Solutions>, ISolutionsDatastore
  {
    public SolutionsDatastore(IDbConnectionFactory dbConnectionFactory, ILogger<SolutionsDatastore> logger, ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<Solutions> ByFramework(string frameworkId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select soln.* from Solutions soln
join FrameworkSolution fs on fs.SolutionId = soln.Id
join Frameworks frame on frame.Id = fs.FrameworkId
where frame.Id = @frameworkId
";
        var retval = _dbConnection.Value.Query<Solutions>(sql, new { frameworkId });
        return retval;
      });
    }

    public Solutions ById(string id)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<Solutions>(id);
      });
    }

    public IEnumerable<Solutions> ByOrganisation(string organisationId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
-- select all current versions
select * from Solutions where Id not in 
(
  select PreviousId from Solutions where PreviousId is not null
)
";        return _dbConnection.Value.Query<Solutions>(sql).Where(soln => soln.OrganisationId == organisationId);
      });
    }

    public Solutions Create(Solutions solution)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          solution.Id = UpdateId(solution.Id);
          solution.CreatedOn = solution.ModifiedOn = DateTime.UtcNow;
          _dbConnection.Value.Insert(solution, trans);
          trans.Commit();

          return solution;
        }
      });
    }

    public void Update(Solutions solution)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          solution.ModifiedOn = DateTime.UtcNow;
          _dbConnection.Value.Update(solution, trans);
          trans.Commit();
          return 0;
        }
      });
    }

    public void Delete(Solutions solution)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          _dbConnection.Value.Delete(solution, trans);
          trans.Commit();
          return 0;
        }
      });
    }
  }
}
