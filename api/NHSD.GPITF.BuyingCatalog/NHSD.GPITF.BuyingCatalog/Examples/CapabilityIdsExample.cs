using Swashbuckle.AspNetCore.Examples;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class CapabilityIdsExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new[]
      {
        "CAP1",
        "CAP8"
      };
    }
  }
#pragma warning restore CS1591
}
