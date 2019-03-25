using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class CapabilitiesImplementedReviewsDatastore : ReviewsDatastoreBase<CapabilitiesImplementedReviews>, ICapabilitiesImplementedReviewsDatastore
  {
    protected override string ResourceBase { get; } = "/CapabilitiesImplementedReviews";

    public CapabilitiesImplementedReviewsDatastore(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<CapabilitiesImplementedReviews>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmFactory, logger, policy, config)
    {
    }
  }
}
