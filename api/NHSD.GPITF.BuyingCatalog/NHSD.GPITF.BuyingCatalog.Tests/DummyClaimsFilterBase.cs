using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyClaimsFilterBase : ClaimsFilterBase<ClaimsBase>
  {
    public DummyClaimsFilterBase(
      IHttpContextAccessor context,
      ISolutionsDatastore solutionDatastore) :
      base(context, solutionDatastore)
    {
    }
  }
}
