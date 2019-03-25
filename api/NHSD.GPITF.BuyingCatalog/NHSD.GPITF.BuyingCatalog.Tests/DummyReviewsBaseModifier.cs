using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyReviewsBaseModifier : ReviewsBaseModifier<DummyReviewsBase>
  {
    public DummyReviewsBaseModifier(
      IHttpContextAccessor context,
      IContactsDatastore contacts) :
      base(context, contacts)
    {
    }
  }
}
