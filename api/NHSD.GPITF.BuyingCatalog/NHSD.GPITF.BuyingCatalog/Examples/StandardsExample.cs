using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.Examples;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class StandardsExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new Standards
      {
        Id = "8FC9C326-8E3A-4D6B-8A1B-802EE56B2D2A",
        Name = "New example Standard",
        Description = "New example Standard description"
      };
    }
  }
#pragma warning restore CS1591
}
