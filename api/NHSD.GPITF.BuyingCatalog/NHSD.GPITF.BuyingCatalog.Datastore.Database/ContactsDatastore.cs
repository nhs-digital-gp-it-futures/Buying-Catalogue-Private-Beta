using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class ContactsDatastore : DatastoreBase<Contacts>, IContactsDatastore
  {
    public ContactsDatastore(IDbConnectionFactory dbConnectionFactory, ILogger<ContactsDatastore> logger, ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public Contacts ById(string id)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<Contacts>(id);
      });
    }

    public IEnumerable<Contacts> ByOrganisation(string organisationId)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.GetAll<Contacts>().Where(c => c.OrganisationId == organisationId);
      });
    }

    public Contacts ByEmail(string email)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.GetAll<Contacts>().SingleOrDefault(c => c.EmailAddress1.ToLowerInvariant() == email.ToLowerInvariant());
      });
    }
  }
}
