using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IContactsDatastore
  {
    Contacts ById(string id);
    IEnumerable<Contacts> ByOrganisation(string organisationId);
    Contacts ByEmail(string email);
  }
#pragma warning restore CS1591
}
