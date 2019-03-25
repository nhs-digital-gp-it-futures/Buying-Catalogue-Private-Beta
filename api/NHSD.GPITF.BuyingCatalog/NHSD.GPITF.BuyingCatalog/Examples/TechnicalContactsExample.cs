using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.Examples;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class TechnicalContactsExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new TechnicalContacts
      {
        Id = "87a58dbf-ab78-4350-9f3d-9276de3e4be6",
        SolutionId = "A3C6830F-2E7C-4545-A4B9-02D20C4C92E1",
        ContactType = "Lead Contact",
        FirstName = "Jon",
        LastName = "Dough",
        EmailAddress = "jon.dough @tpp.com",
        PhoneNumber = "0300 303 5678"
      };
    }
  }
#pragma warning restore CS1591
}
