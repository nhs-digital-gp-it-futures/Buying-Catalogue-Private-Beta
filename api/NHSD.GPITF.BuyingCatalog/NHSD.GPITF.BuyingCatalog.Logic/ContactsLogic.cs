using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class ContactsLogic : LogicBase, IContactsLogic
  {
    private readonly IContactsDatastore _datastore;
    private readonly IContactsValidator _validator;
    private readonly IContactsFilter _filter;

    public ContactsLogic(
      IContactsDatastore datastore,
      IHttpContextAccessor context,
      IContactsValidator validator,
      IContactsFilter filter) :
      base(context)
    {
      _datastore = datastore;
      _validator = validator;
      _filter = filter;
    }

    public Contacts ById(string id)
    {
      return _filter.Filter(new[] { _datastore.ById(id) }).SingleOrDefault();
    }

    public IEnumerable<Contacts> ByOrganisation(string organisationId)
    {
      return _filter.Filter(_datastore.ByOrganisation(organisationId));
    }

    public Contacts ByEmail(string email)
    {
      return _filter.Filter(new[] { _datastore.ByEmail(email) }).SingleOrDefault();
    }
  }
}
