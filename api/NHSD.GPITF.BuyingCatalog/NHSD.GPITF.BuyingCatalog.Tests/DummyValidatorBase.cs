using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Logic;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyValidatorBase : ValidatorBase<object>
  {
    public DummyValidatorBase(IHttpContextAccessor context, ILogger<DummyValidatorBase> logger) :
      base(context, logger)
    {
    }
  }
}
