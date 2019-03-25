using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedReviewsLogic : ReviewsLogicBase<CapabilitiesImplementedReviews>, ICapabilitiesImplementedReviewsLogic
  {
    public CapabilitiesImplementedReviewsLogic(
      ICapabilitiesImplementedReviewsModifier modifier,
      ICapabilitiesImplementedReviewsDatastore datastore,
      IContactsDatastore contacts,
      ICapabilitiesImplementedReviewsValidator validator,
      ICapabilitiesImplementedReviewsFilter filter,
      IHttpContextAccessor context) :
      base(modifier, datastore, contacts, validator, filter, context)
    {
    }
  }
}
