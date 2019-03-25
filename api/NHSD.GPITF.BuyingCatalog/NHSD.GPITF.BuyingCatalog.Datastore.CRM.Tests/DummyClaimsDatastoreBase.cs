using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.Tests
{
  public sealed class DummyClaimsDatastoreBase : ClaimsDatastoreBase<ClaimsBase>
  {
    protected override string ResourceBase => "ClaimsBase";

    public DummyClaimsDatastoreBase(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<ClaimsBase>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmFactory, logger, policy, config)
    {
    }
  }
}
