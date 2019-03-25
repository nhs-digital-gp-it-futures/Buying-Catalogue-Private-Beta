using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class FrameworksFilter : PassThroughFilterBase<Frameworks>, IFrameworksFilter
  {
    public FrameworksFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
