using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class CapabilitiesImplementedReviewsDatastore : ReviewsDatastoreBase<CapabilitiesImplementedReviews>, ICapabilitiesImplementedReviewsDatastore
  {
    public CapabilitiesImplementedReviewsDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<CapabilitiesImplementedReviewsDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
