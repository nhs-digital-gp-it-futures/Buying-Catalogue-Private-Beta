using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class StandardsApplicableDatastore : ClaimsDatastoreBase<StandardsApplicable>, IStandardsApplicableDatastore, IClaimsDatastore<ClaimsBase>
  {
    protected override string ResourceBase { get; } = "/StandardsApplicable";

    public StandardsApplicableDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<StandardsApplicableDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmConnectionFactory, logger, policy, config)
    {
    }
  }
}
