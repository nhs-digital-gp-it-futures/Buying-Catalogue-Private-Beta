using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class CapabilitiesImplementedDatastore : ClaimsDatastoreBase<CapabilitiesImplemented>, ICapabilitiesImplementedDatastore, IClaimsDatastore<ClaimsBase>
  {
    protected override string ResourceBase { get; } = "/CapabilitiesImplemented";

    public CapabilitiesImplementedDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<CapabilitiesImplementedDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmConnectionFactory, logger, policy, config)
    {
    }
  }
}
