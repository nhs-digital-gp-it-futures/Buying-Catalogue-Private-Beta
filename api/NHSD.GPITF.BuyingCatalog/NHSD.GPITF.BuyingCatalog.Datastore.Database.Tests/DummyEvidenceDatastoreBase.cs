using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database.Tests
{
  public sealed class DummyEvidenceDatastoreBase : EvidenceDatastoreBase<EvidenceBase>
  {
    public DummyEvidenceDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<DummyEvidenceDatastoreBase> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
