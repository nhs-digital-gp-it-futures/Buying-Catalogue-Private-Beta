using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsFilter : PassThroughFilterBase<Standards>, IStandardsFilter
  {
    public StandardsFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
