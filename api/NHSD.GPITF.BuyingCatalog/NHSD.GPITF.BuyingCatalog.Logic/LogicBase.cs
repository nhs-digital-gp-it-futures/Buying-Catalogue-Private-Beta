using Microsoft.AspNetCore.Http;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class LogicBase
  {
    protected readonly IHttpContextAccessor Context;

    public LogicBase(IHttpContextAccessor context)
    {
      Context = context;
    }
  }
}
