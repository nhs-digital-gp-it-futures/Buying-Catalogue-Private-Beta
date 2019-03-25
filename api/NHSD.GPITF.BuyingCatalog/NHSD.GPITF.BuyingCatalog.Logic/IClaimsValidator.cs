using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IClaimsValidator<T> : IValidatorBase<T> where T : ClaimsBase
  {
  }
}
