using FluentValidation;
using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class LinkManagerLogic : LogicBase, ILinkManagerLogic
  {
    private readonly ILinkManagerDatastore _datastore;
    private readonly ILinkManagerValidator _validator;

    public LinkManagerLogic(
      ILinkManagerDatastore datastore,
      IHttpContextAccessor context,
      ILinkManagerValidator validator) :
      base(context)
    {
      _datastore = datastore;
      _validator = validator;
    }

    public void FrameworkSolutionCreate(string frameworkId, string solutionId)
    {
      _validator.ValidateAndThrowEx(Context, ruleSet: nameof(ILinkManagerLogic.FrameworkSolutionCreate));
      _datastore.FrameworkSolutionCreate(frameworkId, solutionId);
    }
  }
}
