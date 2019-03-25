using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyClaimsBase : ClaimsBase
  {
    public override string QualityId => throw new System.NotImplementedException();
  }
}
