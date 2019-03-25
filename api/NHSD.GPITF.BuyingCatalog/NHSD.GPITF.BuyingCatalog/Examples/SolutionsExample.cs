using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.Examples;
using System;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class SolutionsExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new Solutions
      {
        Id = "A3C6830F-2E7C-4545-A4B9-02D20C4C92E1",
        PreviousId = "12968eb4-4160-4ec5-8bb7-3deca7c3f53b",
        OrganisationId = "9c2fd4d4-98ca-41d7-b8f4-1ad6f6702127",
        CreatedById = "371b20b7-3bed-487d-aa5f-995f5924e579",
        CreatedOn = new DateTime(2017, 12, 25, 06, 45, 0),
        ModifiedById = "371b20b7-3bed-487d-aa5f-995f5924e579",
        ModifiedOn = new DateTime(2018, 12, 31, 17, 25, 0),
        Version = "2.0",
        Status = SolutionStatus.Draft,
        Name = "Really Kool Document Manager",
        Description = "Does Really Kool document management"
      };
    }
  }
#pragma warning restore CS1591
}
