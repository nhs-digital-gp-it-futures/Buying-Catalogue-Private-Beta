using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableReviewsFilter : ReviewsFilterBase<IEnumerable<StandardsApplicableReviews>>, IStandardsApplicableReviewsFilter
  {
    public StandardsApplicableReviewsFilter(
      IStandardsApplicableEvidenceDatastore evidenceDatastore,
      IStandardsApplicableDatastore claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context) :
      base((IEvidenceDatastore<EvidenceBase>)evidenceDatastore, (IClaimsDatastore<ClaimsBase>)claimDatastore, solutionDatastore, context)
    {
    }
  }
}
