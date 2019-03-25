using Microsoft.AspNetCore.Mvc;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.IO;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IEvidenceBlobStoreDatastore
  {
    void PrepareForSolution(IClaimsInfoProvider claimsInfoProvider, string solutionId);
    string AddEvidenceForClaim(IClaimsInfoProvider claimsInfoProvider, string claimId, Stream file, string filename, string subFolder = null);
    FileStreamResult GetFileStream(IClaimsInfoProvider claimsInfoProvider, string claimId, string uniqueId);
    IEnumerable<BlobInfo> EnumerateFolder(IClaimsInfoProvider claimsInfoProvider, string claimId, string subFolder = null);
    IEnumerable<ClaimBlobInfoMap> EnumerateClaimFolderTree(IClaimsInfoProvider claimsInfoProvider, string solutionId);
  }
#pragma warning restore CS1591
}
