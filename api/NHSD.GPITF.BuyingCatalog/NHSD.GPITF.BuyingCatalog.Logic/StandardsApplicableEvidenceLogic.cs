using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableEvidenceLogic : EvidenceLogicBase<StandardsApplicableEvidence>, IStandardsApplicableEvidenceLogic
  {
    public StandardsApplicableEvidenceLogic(
      IStandardsApplicableEvidenceModifier modifier,
      IStandardsApplicableEvidenceDatastore datastore,
      IContactsDatastore contacts,
      IStandardsApplicableEvidenceValidator validator,
      IStandardsApplicableEvidenceFilter filter,
      IHttpContextAccessor context) :
      base(modifier, datastore, contacts, validator, filter, context)
    {
    }
  }
}
