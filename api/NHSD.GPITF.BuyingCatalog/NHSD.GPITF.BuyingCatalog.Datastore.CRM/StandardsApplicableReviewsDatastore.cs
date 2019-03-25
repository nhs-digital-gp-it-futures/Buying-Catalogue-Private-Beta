using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class StandardsApplicableReviewsDatastore : ReviewsDatastoreBase<StandardsApplicableReviews>, IStandardsApplicableReviewsDatastore
  {
    protected override string ResourceBase { get; } = "/StandardsApplicableReviews";

    public StandardsApplicableReviewsDatastore(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<StandardsApplicableReviews>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmFactory, logger, policy, config)
    {
    }
  }
}
