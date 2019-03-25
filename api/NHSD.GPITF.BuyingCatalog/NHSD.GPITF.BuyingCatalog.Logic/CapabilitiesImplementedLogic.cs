using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedLogic : ClaimsLogicBase<CapabilitiesImplemented>, ICapabilitiesImplementedLogic
  {
    public CapabilitiesImplementedLogic(
      ICapabilitiesImplementedModifier modifier,
      ICapabilitiesImplementedDatastore datastore,
      ICapabilitiesImplementedValidator validator,
      ICapabilitiesImplementedFilter filter,
      IHttpContextAccessor context
      ) :
      base(modifier, datastore, validator, filter, context)
    {
    }
  }
}
