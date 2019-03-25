using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedReviewsModifier : ReviewsBaseModifier<CapabilitiesImplementedReviews>, ICapabilitiesImplementedReviewsModifier
  {
    public CapabilitiesImplementedReviewsModifier(
      IHttpContextAccessor context,
      IContactsDatastore contacts) :
      base(context, contacts)
    {
    }
  }
}
