using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IReviewsBaseModifier<T> where T : ReviewsBase
  {
    void ForCreate(T input);
    void ForUpdate(T input);
  }
}
