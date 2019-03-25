using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class ContactsDatastore : CachedDatastore<Contacts>, IContactsDatastore
  {
    public ContactsDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<ContactsDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config,
      IDatastoreCache cache) :
      base(crmConnectionFactory, logger, policy, config, cache)
    {
    }

    private string ResourceBase { get; } = "/Contacts";

    public Contacts ByEmail(string email)
    {
      return GetInternal(() =>
      {
        return Get($"{ResourceBase}/ByEmail/{email}");
      });
    }

    public Contacts ById(string id)
    {
      return GetInternal(() =>
      {
        var request = GetRequest($"{ResourceBase}/ById/{id}");
        var retval = GetResponse<Contacts>(request);

        return retval;
      });
    }

    public IEnumerable<Contacts> ByOrganisation(string organisationId)
    {
      return GetInternal(() =>
      {
        return GetAll($"{ResourceBase}/ByOrganisation/{organisationId}");
      });
    }
  }
}
