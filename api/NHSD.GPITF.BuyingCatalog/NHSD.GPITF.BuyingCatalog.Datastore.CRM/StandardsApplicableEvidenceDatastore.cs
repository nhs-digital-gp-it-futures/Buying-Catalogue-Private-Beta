using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class StandardsApplicableEvidenceDatastore : EvidenceDatastoreBase<StandardsApplicableEvidence>, IStandardsApplicableEvidenceDatastore
  {
    protected override string ResourceBase { get; } = "/StandardsApplicableEvidence";

    public StandardsApplicableEvidenceDatastore(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<StandardsApplicableEvidence>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmFactory, logger, policy, config)
    {
    }
  }
}
