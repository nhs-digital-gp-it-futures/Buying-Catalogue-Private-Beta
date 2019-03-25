using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedFilter : ClaimsFilterBase<CapabilitiesImplemented>, ICapabilitiesImplementedFilter
  {
    public CapabilitiesImplementedFilter(
      IHttpContextAccessor context,
      ISolutionsDatastore solutionDatastore) :
      base(context, solutionDatastore)
    {
    }
  }
}
