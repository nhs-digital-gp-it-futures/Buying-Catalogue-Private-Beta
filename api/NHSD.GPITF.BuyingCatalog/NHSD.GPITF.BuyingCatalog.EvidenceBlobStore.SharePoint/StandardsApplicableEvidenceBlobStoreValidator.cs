using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public sealed class StandardsApplicableEvidenceBlobStoreValidator : ClaimsEvidenceBlobStoreValidatorBase, IStandardsApplicableEvidenceBlobStoreValidator
  {
    public StandardsApplicableEvidenceBlobStoreValidator(
      IHttpContextAccessor context,
      ILogger<StandardsApplicableEvidenceBlobStoreValidator> logger,
      ISolutionsDatastore solutionsDatastore,
      IStandardsApplicableDatastore claimsDatastore) :
      base(context, logger, solutionsDatastore, (IClaimsDatastore<ClaimsBase>)claimsDatastore)
    {
    }
  }
}
