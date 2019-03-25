using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database.Tests
{
  public sealed class DummyReviewsDatastoreBase : ReviewsDatastoreBase<ReviewsBase>
  {
    public DummyReviewsDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ReviewsDatastoreBase<ReviewsBase>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
