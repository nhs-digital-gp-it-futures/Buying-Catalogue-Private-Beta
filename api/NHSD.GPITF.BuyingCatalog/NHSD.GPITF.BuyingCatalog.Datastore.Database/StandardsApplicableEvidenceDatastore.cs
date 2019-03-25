using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class StandardsApplicableEvidenceDatastore : EvidenceDatastoreBase<StandardsApplicableEvidence>, IStandardsApplicableEvidenceDatastore
  {
    public StandardsApplicableEvidenceDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<StandardsApplicableEvidenceDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
