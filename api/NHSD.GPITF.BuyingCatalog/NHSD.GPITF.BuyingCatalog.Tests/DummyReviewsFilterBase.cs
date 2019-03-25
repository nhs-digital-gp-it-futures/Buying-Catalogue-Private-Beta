using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyReviewsFilterBase : ReviewsFilterBase<IEnumerable<ReviewsBase>>
  {
    public DummyReviewsFilterBase(
      IEvidenceDatastore<EvidenceBase> evidenceDatastore,
      IClaimsDatastore<ClaimsBase> claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context) :
      base(evidenceDatastore, claimDatastore, solutionDatastore, context)
    {
    }
  }
}
