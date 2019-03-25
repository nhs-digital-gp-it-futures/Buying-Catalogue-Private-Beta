using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableReviewsValidator : ReviewsValidatorBase<StandardsApplicableReviews>, IStandardsApplicableReviewsValidator
  {
    public StandardsApplicableReviewsValidator(
      IStandardsApplicableReviewsDatastore datastore,
      IStandardsApplicableEvidenceDatastore evidenceDatastore,
      IStandardsApplicableDatastore claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context,
      ILogger<StandardsApplicableReviewsValidator> logger) :
      base((IReviewsDatastore<ReviewsBase>)datastore, (IEvidenceDatastore<EvidenceBase>)evidenceDatastore, (IClaimsDatastore<ClaimsBase>)claimDatastore, solutionDatastore, context, logger)
    {
    }

    protected override SolutionStatus SolutionReviewStatus => SolutionStatus.StandardsCompliance;
  }
}
