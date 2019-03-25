using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public class EvidenceBlobStoreValidator : ValidatorBase<string>, IEvidenceBlobStoreValidator
  {
    public EvidenceBlobStoreValidator(
      IHttpContextAccessor context,
      ILogger<EvidenceBlobStoreValidator> logger) :
      base(context, logger)
    {
      // solutionId
      RuleSet(nameof(IEvidenceBlobStoreLogic.PrepareForSolution), () =>
      {
        MustBeAdminOrSupplier();
      });

      // claimId
      RuleSet(nameof(IEvidenceBlobStoreLogic.AddEvidenceForClaim), () =>
      {
        MustBeAdminOrSupplier();
      });

      // claimId
      RuleSet(nameof(IEvidenceBlobStoreLogic.EnumerateFolder), () =>
      {
        MustBeAdminOrSupplier();
      });
    }
  }
}
