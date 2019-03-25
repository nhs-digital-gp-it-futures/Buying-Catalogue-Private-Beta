using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IClaimsFilter<T> : IFilter<T> where T : ClaimsBase
  {
  }
}
