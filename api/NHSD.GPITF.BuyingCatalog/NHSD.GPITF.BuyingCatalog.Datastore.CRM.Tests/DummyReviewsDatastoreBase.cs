using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.Tests
{
  public sealed class DummyReviewsDatastoreBase : ReviewsDatastoreBase<ReviewsBase>
  {
    protected override string ResourceBase => "ReviewsBase";

    public DummyReviewsDatastoreBase(
      IRestClientFactory dbConnectionFactory,
      ILogger<ReviewsDatastoreBase<ReviewsBase>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(dbConnectionFactory, logger, policy, config)
    {
    }
  }
}
