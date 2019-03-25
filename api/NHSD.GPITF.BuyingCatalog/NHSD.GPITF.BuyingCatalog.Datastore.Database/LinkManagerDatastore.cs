using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class LinkManagerDatastore : DatastoreBase<object>, ILinkManagerDatastore
  {
    public LinkManagerDatastore(IDbConnectionFactory dbConnectionFactory, ILogger<LinkManagerDatastore> logger, ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public void FrameworkSolutionCreate(string frameworkId, string solutionId)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          var entity = new FrameworkSolution { FrameworkId = frameworkId, SolutionId = solutionId };
          _dbConnection.Value.Insert(entity, trans);
          trans.Commit();
          return 0;
        }
      });
    }
  }
}
