using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.Examples;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class FrameworksExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new Frameworks
      {
        Id = "34679272-20DF-4867-B352-CB57A520FA53",
        Name = "New example Framework",
        Description = "New example Framework description"
      };
    }
  }
#pragma warning restore CS1591
}
