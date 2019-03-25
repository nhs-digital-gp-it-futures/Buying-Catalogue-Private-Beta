using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.Examples;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class CapabilitiesExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new Capabilities
      {
        Id = "7FE43F88-4EB3-47C9-9978-1ACCC8CAD8A1",
        Name = "New example Capability",
        Description = "New example Capability description"
      };
    }
  }
#pragma warning restore CS1591
}
