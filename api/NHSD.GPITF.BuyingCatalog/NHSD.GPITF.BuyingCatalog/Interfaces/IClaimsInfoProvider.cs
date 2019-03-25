using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IClaimsInfoProvider
  {
    ClaimsBase GetClaimById(string claimId);
    IEnumerable<ClaimsBase> GetClaimBySolution(string solutionId);
    string GetFolderName();
    string GetFolderClaimName(ClaimsBase claim);
    string GetCapabilityFolderName();
    string GetStandardsFolderName();
    IEnumerable<Quality> GetAllQualities();
  }
#pragma warning restore CS1591
}
