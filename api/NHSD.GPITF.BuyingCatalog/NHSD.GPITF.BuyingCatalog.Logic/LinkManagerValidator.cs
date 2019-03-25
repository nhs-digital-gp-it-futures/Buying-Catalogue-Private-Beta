using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class LinkManagerValidator : ValidatorBase<object>, ILinkManagerValidator
  {
    public LinkManagerValidator(IHttpContextAccessor context, ILogger<LinkManagerValidator> logger) :
      base(context, logger)
    {
      RuleSet(nameof(ILinkManagerLogic.FrameworkSolutionCreate), () =>
      {
        MustBeAdmin();
      });
    }
  }
}
