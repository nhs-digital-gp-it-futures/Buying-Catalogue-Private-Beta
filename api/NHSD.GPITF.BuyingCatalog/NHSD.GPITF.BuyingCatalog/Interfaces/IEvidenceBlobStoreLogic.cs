using Microsoft.AspNetCore.Mvc;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.IO;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IEvidenceBlobStoreLogic
  {
    void PrepareForSolution(string solutionId);
    string AddEvidenceForClaim(string claimId, Stream file, string filename, string subFolder = null);
    FileStreamResult GetFileStream(string claimId, string uniqueId);
    IEnumerable<BlobInfo> EnumerateFolder(string claimId, string subFolder = null);
    IEnumerable<ClaimBlobInfoMap> EnumerateClaimFolderTree(string solutionId);
  }
#pragma warning restore CS1591
}
