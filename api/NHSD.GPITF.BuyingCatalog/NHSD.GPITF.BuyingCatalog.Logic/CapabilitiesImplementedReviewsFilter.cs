using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedReviewsFilter : ReviewsFilterBase<IEnumerable<CapabilitiesImplementedReviews>>, ICapabilitiesImplementedReviewsFilter
  {
    public CapabilitiesImplementedReviewsFilter(
      ICapabilitiesImplementedEvidenceDatastore evidenceDatastore,
      ICapabilitiesImplementedDatastore claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context) :
      base((IEvidenceDatastore<EvidenceBase>)evidenceDatastore, (IClaimsDatastore<ClaimsBase>)claimDatastore, solutionDatastore, context)
    {
    }
  }
}
