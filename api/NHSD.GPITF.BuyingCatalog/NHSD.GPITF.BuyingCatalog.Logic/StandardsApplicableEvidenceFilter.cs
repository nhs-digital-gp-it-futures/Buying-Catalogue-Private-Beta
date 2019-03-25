using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableEvidenceFilter : EvidenceFilterBase<IEnumerable<StandardsApplicableEvidence>>, IStandardsApplicableEvidenceFilter
  {
    public StandardsApplicableEvidenceFilter(
      IStandardsApplicableDatastore claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context) :
      base((IClaimsDatastore<ClaimsBase>)claimDatastore, solutionDatastore, context)
    {
    }
  }
}
