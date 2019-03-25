using Microsoft.AspNetCore.Http;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class PassThroughFilterBase<T> : FilterBase<T>
  {
    public PassThroughFilterBase(IHttpContextAccessor context) :
      base(context)
    {
    }

    public override T Filter(T input)
    {
      return input;
    }
  }
}
