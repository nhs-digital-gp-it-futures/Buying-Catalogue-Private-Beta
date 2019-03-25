using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyReviewsLogicBase : ReviewsLogicBase<ReviewsBase>
  {
    public DummyReviewsLogicBase(
      IReviewsBaseModifier<ReviewsBase> modifier,
      IReviewsDatastore<ReviewsBase> datastore,
      IContactsDatastore contacts,
      IReviewsValidator<ReviewsBase> validator,
      IReviewsFilter<IEnumerable<ReviewsBase>> filter,
      IHttpContextAccessor context) :
      base(modifier,datastore, contacts, validator, filter, context)
    {
    }
  }
}
