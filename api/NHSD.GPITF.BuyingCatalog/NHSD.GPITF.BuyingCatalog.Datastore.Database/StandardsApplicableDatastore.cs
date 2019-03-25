using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class StandardsApplicableDatastore : ClaimsDatastoreBase<StandardsApplicable>, IStandardsApplicableDatastore
  {
    public StandardsApplicableDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<StandardsApplicableDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
