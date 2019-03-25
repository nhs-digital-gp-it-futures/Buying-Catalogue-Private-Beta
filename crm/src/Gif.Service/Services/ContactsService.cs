#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using System.Collections.Generic;
using System.Linq;
using Contact = Gif.Service.Models.Contact;

namespace Gif.Service.Services
{
    public class ContactsService : ServiceBase<Contact>, IContactsDatastore
    {
        public ContactsService(IRepository repository) : base(repository)
        {
        }

        public Contact ByEmail(string email)
        {
            email = $"'{email}'";

            var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Email") {FilterName = "emailaddress1", FilterValue = email},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"},
                new CrmFilterAttribute("BuyingCatalogueContact") {FilterName = "cc_buyingcataloguecontact", FilterValue = "true"}
            };

            var appJson = Repository.RetrieveMultiple(new Contact().GetQueryString(null, filterAttributes), out Count);
            var contactJson = appJson?.FirstOrDefault();

            return (contactJson == null) ? null : new Contact(contactJson);
        }

        public Contact ById(string id)
        {
            var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("ContactId") {FilterName = "contactid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"},
                new CrmFilterAttribute("BuyingCatalogueContact") {FilterName = "cc_buyingcataloguecontact", FilterValue = "true"}
            };

            var appJson = Repository.RetrieveMultiple(new Contact().GetQueryString(null, filterAttributes), out Count);
            var contactJson = appJson?.FirstOrDefault();

            return (contactJson == null) ? null : new Contact(contactJson);
        }

        public IEnumerable<Contact> ByOrganisation(string organisationId)
        {
            var contacts = new List<Contact>();

            var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Organisation") {FilterName = "_parentcustomerid_value", FilterValue = organisationId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"},
                new CrmFilterAttribute("BuyingCatalogueContact") {FilterName = "cc_buyingcataloguecontact", FilterValue = "true"}
            };

            var appJson = Repository.RetrieveMultiple(new Contact().GetQueryString(null, filterAttributes, true, true), out Count);

            foreach (var contact in appJson.Children())
            {
                contacts.Add(new Contact(contact));
            }

            Count = contacts.Count();

            return contacts;
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
