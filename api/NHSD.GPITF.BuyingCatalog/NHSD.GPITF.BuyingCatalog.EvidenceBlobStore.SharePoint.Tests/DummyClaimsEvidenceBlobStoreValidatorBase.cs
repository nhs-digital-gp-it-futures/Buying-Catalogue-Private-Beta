using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint.Tests
{
  public sealed class DummyClaimsEvidenceBlobStoreValidatorBase : ClaimsEvidenceBlobStoreValidatorBase
  {
    public DummyClaimsEvidenceBlobStoreValidatorBase(
      IHttpContextAccessor context,
      ILogger<DummyClaimsEvidenceBlobStoreValidatorBase> logger,
      ISolutionsDatastore solutionsDatastore,
      IClaimsDatastore<ClaimsBase> claimsDatastore) :
      base(context, logger, solutionsDatastore, claimsDatastore)
    {
    }
  }
}
