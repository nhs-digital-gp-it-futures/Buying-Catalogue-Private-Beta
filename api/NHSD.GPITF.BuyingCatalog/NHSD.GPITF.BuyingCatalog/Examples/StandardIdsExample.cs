using Swashbuckle.AspNetCore.Examples;

namespace NHSD.GPITF.BuyingCatalog.Examples
{
#pragma warning disable CS1591
  public sealed class StandardIdsExample : IExamplesProvider
  {
    public object GetExamples()
    {
      return new[]
      {
        "INT12",
        "INT14",
        "INT16"
      };
    }
  }
#pragma warning restore CS1591
}
