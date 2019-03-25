using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyReviewsValidatorBase : ReviewsValidatorBase<DummyReviewsBase>
  {
    public DummyReviewsValidatorBase(
      IReviewsDatastore<ReviewsBase> reviewsDatastore,
      IEvidenceDatastore<EvidenceBase> evidenceDatastore,
      IClaimsDatastore<ClaimsBase> claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context,
      ILogger<DummyReviewsValidatorBase> logger) :
      base(reviewsDatastore, evidenceDatastore, claimDatastore, solutionDatastore, context, logger)
    {
    }

    protected override SolutionStatus SolutionReviewStatus => SolutionStatus.Failed;
  }
}
