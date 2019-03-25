using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableLogic : ClaimsLogicBase<StandardsApplicable>, IStandardsApplicableLogic
  {
    public StandardsApplicableLogic(
      IStandardsApplicableModifier modifier,
      IStandardsApplicableDatastore datastore,
      IStandardsApplicableValidator validator,
      IStandardsApplicableFilter filter,
      IHttpContextAccessor context) :
      base(modifier, datastore, validator, filter, context)
    {
    }

    public override void Update(StandardsApplicable claim)
    {
      _validator.ValidateAndThrowEx(claim, ruleSet: nameof(IClaimsLogic<StandardsApplicable>.Update));

      ((IStandardsApplicableModifier)_modifier).ForUpdate(claim);

      _datastore.Update(claim);
    }
  }
}
