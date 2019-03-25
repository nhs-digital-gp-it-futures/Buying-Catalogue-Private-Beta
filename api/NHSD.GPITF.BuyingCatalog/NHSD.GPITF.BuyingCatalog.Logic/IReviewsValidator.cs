using FluentValidation;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IReviewsValidator<T> : IValidatorBase<T> where T : ReviewsBase
  {
  }
}