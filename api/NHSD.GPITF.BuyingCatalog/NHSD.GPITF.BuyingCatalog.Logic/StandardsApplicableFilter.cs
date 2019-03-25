using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableFilter : ClaimsFilterBase<StandardsApplicable>, IStandardsApplicableFilter
  {
    public StandardsApplicableFilter(
      IHttpContextAccessor context,
      ISolutionsDatastore solutionDatastore) :
      base(context, solutionDatastore)
    {
    }
  }
}
