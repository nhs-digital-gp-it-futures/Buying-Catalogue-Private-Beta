using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class CapabilitiesImplementedDatastore : ClaimsDatastoreBase<CapabilitiesImplemented>, ICapabilitiesImplementedDatastore
  {
    public CapabilitiesImplementedDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<CapabilitiesImplementedDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
