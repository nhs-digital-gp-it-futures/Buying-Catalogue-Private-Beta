using Gif.Service.Models;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IOrganisationsDatastore
  {
    Organisation ByContact(string contactId);
    Organisation ById(string organisationId);
  }
#pragma warning restore CS1591
}
