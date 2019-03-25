using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableReviewsLogic : ReviewsLogicBase<StandardsApplicableReviews>, IStandardsApplicableReviewsLogic
  {
    public StandardsApplicableReviewsLogic(
      IStandardsApplicableReviewsModifier modifier,
      IStandardsApplicableReviewsDatastore datastore,
      IContactsDatastore contacts,
      IStandardsApplicableReviewsValidator validator,
      IStandardsApplicableReviewsFilter filter,
      IHttpContextAccessor context) :
      base(modifier, datastore, contacts, validator, filter, context)
    {
    }
  }
}
