using Gif.Service.Models;
using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IContactsDatastore : IDatastoreBase<Contact>
  {
    Contact ById(string id);
    IEnumerable<Contact> ByOrganisation(string organisationId);
    Contact ByEmail(string email);
  }
#pragma warning restore CS1591
}
