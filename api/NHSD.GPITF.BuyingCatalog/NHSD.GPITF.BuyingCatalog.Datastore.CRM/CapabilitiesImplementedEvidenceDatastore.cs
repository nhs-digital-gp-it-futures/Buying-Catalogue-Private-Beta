using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class CapabilitiesImplementedEvidenceDatastore : EvidenceDatastoreBase<CapabilitiesImplementedEvidence>, ICapabilitiesImplementedEvidenceDatastore
  {
    protected override string ResourceBase { get; } = "/CapabilitiesImplementedEvidence";

    public CapabilitiesImplementedEvidenceDatastore(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<CapabilitiesImplementedEvidence>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmFactory, logger, policy, config)
    {
    }
  }
}
