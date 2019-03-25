using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableEvidenceValidator : EvidenceValidatorBase<StandardsApplicableEvidence>, IStandardsApplicableEvidenceValidator
  {
    public StandardsApplicableEvidenceValidator(
      IStandardsApplicableEvidenceDatastore evidenceDatastore,
      IStandardsApplicableDatastore claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context,
      ILogger<StandardsApplicableEvidenceValidator> logger) :
      base(evidenceDatastore, (IClaimsDatastore<ClaimsBase>)claimDatastore, solutionDatastore, context, logger)
    {
    }

    protected override SolutionStatus SolutionReviewStatus => SolutionStatus.StandardsCompliance;
  }
}
