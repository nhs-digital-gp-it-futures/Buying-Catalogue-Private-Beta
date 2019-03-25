using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.Tests
{
  public sealed class DummyEvidenceDatastoreBase : EvidenceDatastoreBase<EvidenceBase>
  {
    protected override string ResourceBase => "EvidenceBase";

    public DummyEvidenceDatastoreBase(
      IRestClientFactory crmFactory, 
      ILogger<DatastoreBase<EvidenceBase>> logger, 
      ISyncPolicyFactory policy,
      IConfiguration config) : 
      base(crmFactory, logger, policy, config)
    {
    }
  }
}
