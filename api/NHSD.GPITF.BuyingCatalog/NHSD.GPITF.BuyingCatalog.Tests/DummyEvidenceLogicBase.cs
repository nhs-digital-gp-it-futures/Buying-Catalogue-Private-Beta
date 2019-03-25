using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyEvidenceLogicBase : EvidenceLogicBase<EvidenceBase>
  {
    public DummyEvidenceLogicBase(
      IEvidenceBaseModifier<EvidenceBase> modifier,
      IEvidenceDatastore<EvidenceBase> datastore,
      IContactsDatastore contacts,
      IEvidenceValidator<EvidenceBase> validator,
      IEvidenceFilter<IEnumerable<EvidenceBase>> filter,
      IHttpContextAccessor context) :
      base(modifier, datastore, contacts, validator, filter, context)
    {
    }
  }
}
