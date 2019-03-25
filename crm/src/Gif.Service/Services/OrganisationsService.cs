#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
  public class OrganisationsService : ServiceBase<Organisation>, IOrganisationsDatastore
  {
    public OrganisationsService(IRepository repository) : base(repository)
    {
    }

    public Organisation ByContact(string contactId)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("ContactId") {FilterName = "contactid", FilterValue = contactId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Contact().GetQueryString(null, filterAttributes), out Count);
      var contactJson = appJson?.FirstOrDefault();

      if (contactJson == null)
      {
        return null;
      }

      var contact = new Contact(contactJson);

      return ById(contact.OrganisationId.ToString());
    }

    public Organisation ById(string organisationId)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("OrganisationId") {FilterName = "accountid", FilterValue = organisationId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Organisation().GetQueryString(null, filterAttributes), out Count);
      var organisation = appJson?.FirstOrDefault();

      return (organisation == null) ? null : new Organisation(organisation);
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member