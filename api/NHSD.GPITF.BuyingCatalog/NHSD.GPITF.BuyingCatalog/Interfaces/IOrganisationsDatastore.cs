using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IOrganisationsDatastore
  {
    Organisations ByContact(string contactId);
    Organisations ById(string organisationId);
  }
#pragma warning restore CS1591
}
