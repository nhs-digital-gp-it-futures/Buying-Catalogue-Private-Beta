using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyEvidenceBaseModifier : EvidenceBaseModifier<EvidenceBase>
  {
    public DummyEvidenceBaseModifier(
      IHttpContextAccessor context,
      IContactsDatastore contacts) :
      base(context, contacts)
    {
    }
  }
}
