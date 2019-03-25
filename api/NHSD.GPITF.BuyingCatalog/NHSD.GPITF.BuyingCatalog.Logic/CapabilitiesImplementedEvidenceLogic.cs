using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedEvidenceLogic : EvidenceLogicBase<CapabilitiesImplementedEvidence>, ICapabilitiesImplementedEvidenceLogic
  {
    public CapabilitiesImplementedEvidenceLogic(
      ICapabilitiesImplementedEvidenceModifier modifier,
      ICapabilitiesImplementedEvidenceDatastore datastore,
      IContactsDatastore contacts,
      ICapabilitiesImplementedEvidenceValidator validator,
      ICapabilitiesImplementedEvidenceFilter filter,
      IHttpContextAccessor context) :
      base(modifier, datastore, contacts, validator, filter, context)
    {
    }
  }
}
