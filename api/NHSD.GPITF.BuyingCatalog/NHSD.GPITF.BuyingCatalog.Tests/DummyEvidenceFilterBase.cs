using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyEvidenceFilterBase : EvidenceFilterBase<IEnumerable<EvidenceBase>>
  {
    public DummyEvidenceFilterBase(
      IClaimsDatastore<ClaimsBase> claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context) :
      base(claimDatastore, solutionDatastore, context)
    {
    }
  }
}
