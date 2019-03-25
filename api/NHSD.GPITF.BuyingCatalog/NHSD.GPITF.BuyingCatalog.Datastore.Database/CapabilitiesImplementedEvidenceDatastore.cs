using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class CapabilitiesImplementedEvidenceDatastore : EvidenceDatastoreBase<CapabilitiesImplementedEvidence>, ICapabilitiesImplementedEvidenceDatastore
  {
    public CapabilitiesImplementedEvidenceDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<CapabilitiesImplementedEvidenceDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
