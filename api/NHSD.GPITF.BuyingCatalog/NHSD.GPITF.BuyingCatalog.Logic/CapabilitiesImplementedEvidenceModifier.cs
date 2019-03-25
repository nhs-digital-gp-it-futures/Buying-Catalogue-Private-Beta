using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedEvidenceModifier : EvidenceBaseModifier<CapabilitiesImplementedEvidence>, ICapabilitiesImplementedEvidenceModifier
  {
    public CapabilitiesImplementedEvidenceModifier(
      IHttpContextAccessor context,
      IContactsDatastore contacts) :
      base(context, contacts)
    {
    }
  }
}
