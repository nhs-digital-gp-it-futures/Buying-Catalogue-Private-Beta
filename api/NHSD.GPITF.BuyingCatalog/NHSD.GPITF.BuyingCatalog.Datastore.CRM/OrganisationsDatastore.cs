using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class OrganisationsDatastore : CachedDatastore<Organisations>, IOrganisationsDatastore
  {
    public OrganisationsDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<OrganisationsDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config,
      IDatastoreCache cache) :
      base(crmConnectionFactory, logger, policy, config, cache)
    {
    }

    private string ResourceBase { get; } = "/Organisations";

    public Organisations ByContact(string contactId)
    {
      return GetInternal(() =>
      {
        return Get($"{ResourceBase}/ByContact/{contactId}");
      });
    }

    public Organisations ById(string id)
    {
      return GetInternal(() =>
      {
        return Get($"{ResourceBase}/ById/{id}");
      });
    }
  }
}
