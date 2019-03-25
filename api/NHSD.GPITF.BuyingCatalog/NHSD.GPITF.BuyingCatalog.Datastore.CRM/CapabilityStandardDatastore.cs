using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class CapabilityStandardDatastore : CachedDatastore<CapabilityStandard>, ICapabilityStandardDatastore
  {
    public CapabilityStandardDatastore(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<CapabilityStandard>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config,
      IDatastoreCache cache) :
      base(crmFactory, logger, policy, config, cache)
    {
    }

    private string ResourceBase { get; } = "/CapabilityStandards";

    public IEnumerable<CapabilityStandard> GetAll()
    {
      return GetInternal(() =>
      {
        return GetAll($"{ResourceBase}");
      });
    }
  }
}
