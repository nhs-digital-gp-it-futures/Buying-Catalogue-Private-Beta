using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class StandardsApplicableReviewsDatastore : ReviewsDatastoreBase<StandardsApplicableReviews>, IStandardsApplicableReviewsDatastore
  {
    public StandardsApplicableReviewsDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<StandardsApplicableReviewsDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
