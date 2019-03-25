using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public sealed class CapabilitiesImplementedEvidenceBlobStoreValidator : ClaimsEvidenceBlobStoreValidatorBase, ICapabilitiesImplementedEvidenceBlobStoreValidator
  {
    public CapabilitiesImplementedEvidenceBlobStoreValidator(
      IHttpContextAccessor context,
      ILogger<CapabilitiesImplementedEvidenceBlobStoreValidator> logger,
      ISolutionsDatastore solutionsDatastore,
      ICapabilitiesImplementedDatastore claimsDatastore) :
      base(context, logger, solutionsDatastore, (IClaimsDatastore<ClaimsBase>)claimsDatastore)
    {
    }
  }
}
