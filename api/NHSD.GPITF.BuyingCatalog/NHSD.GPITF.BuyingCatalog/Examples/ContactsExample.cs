using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.Examples;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class ContactsExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new Contacts
      {
        Id = "70B7D8DE-494B-43AD-A713-8E6A6472FB14",
        OrganisationId = "067A715D-AA0D-4907-B6B8-366611CB1FC4",
        FirstName = "Ken",
        LastName = "Grey",
        EmailAddress1 = "Ken.Grey@TPP.com",
        PhoneNumber1 = "0300 303 5678"
      };
    }
  }
#pragma warning restore CS1591
}
