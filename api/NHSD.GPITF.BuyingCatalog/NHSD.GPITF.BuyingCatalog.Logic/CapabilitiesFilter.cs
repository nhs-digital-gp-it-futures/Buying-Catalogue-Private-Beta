using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesFilter : PassThroughFilterBase<Capabilities>, ICapabilitiesFilter
  {
    public CapabilitiesFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
