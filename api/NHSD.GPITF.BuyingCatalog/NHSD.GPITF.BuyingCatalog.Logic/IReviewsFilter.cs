using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IReviewsFilter<T> : IFilter<T> where T : IEnumerable<ReviewsBase>
  {
  }
}
