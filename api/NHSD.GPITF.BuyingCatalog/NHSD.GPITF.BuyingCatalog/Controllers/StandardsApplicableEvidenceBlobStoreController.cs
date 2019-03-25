using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.OperationFilters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Manage standards evidence
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class StandardsApplicableEvidenceBlobStoreController : EvidenceBlobStoreControllerBase
  {
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public StandardsApplicableEvidenceBlobStoreController(IStandardsApplicableEvidenceBlobStoreLogic logic) :
      base(logic)
    {
    }

    /// <summary>
    /// Upload a file to support a claim
    /// If the file already exists on the server, then a new version is created
    /// </summary>
    /// <param name="claimId">unique identifier of solution claim</param>
    /// <param name="file">Stream representing file to be uploaded</param>
    /// <param name="filename">name of file on the server</param>
    /// <param name="subFolder">optional sub-folder under claim.  This will be created if it does not exist.</param>
    /// <remarks>
    /// Server side folder structure is something like:
    /// --Organisation
    /// ----Solution
    /// ------Capability Evidence
    /// --------Appointment Management - Citizen
    /// --------Appointment Management - GP
    /// --------Clinical Decision Support
    /// --------[all other claimed capabilities]
    /// ----------Images
    /// ----------PDF
    /// ----------Videos
    /// ----------RTM
    /// ----------Misc
    ///
    /// where subFolder is an optional folder under a claimed capability ie Images, PDF, et al
    /// </remarks>
    /// <returns>unique identifier of file</returns>
    [HttpPost]
    [Route("AddEvidenceForClaim")]
    [SwaggerOperationFilter(typeof(EvidenceForClaimFileUploadOperation))]
    [ValidateModelState]
    [DisableRequestSizeLimit]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(string), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Claim not found in CRM")]
    public IActionResult AddEvidenceForClaim([Required]string claimId, [Required]IFormFile file, [Required]string filename, string subFolder = null)
    {
      return AddEvidenceForClaimInternal(claimId, file, filename, subFolder);
    }

    /// <summary>
    /// Download a file which is supporting a claim
    /// </summary>
    /// <param name="claimId">unique identifier of solution claim</param>
    /// <param name="uniqueId">unique identifier of file</param>
    /// <returns>FileResult with suggested file download name based on extUrl</returns>
    [HttpPost]
    [Route("Download/{claimId}")]
    [Produces(typeof(FileResult))]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(FileResult), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Claim not found in CRM")]
    public FileResult Download([FromRoute][Required]string claimId, [FromQuery]string uniqueId)
    {
      return DownloadInternal(claimId, uniqueId);
    }

    /// <summary>
    /// List all files and sub-folders for a claim including folder for claim
    /// </summary>
    /// <param name="claimId">unique identifier of solution claim</param>
    /// <param name="subFolder">optional sub-folder under claim</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <returns>list of BlobInfo - first item is folder for claim</returns>
    [HttpGet]
    [Route("EnumerateFolder/{claimId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<BlobInfo>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Claim not found in CRM")]
    public IActionResult EnumerateFolder([FromRoute][Required]string claimId, [FromQuery]string subFolder, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      return EnumerateFolderInternal(claimId, subFolder, pageIndex, pageSize);
    }

    /// <summary>
    /// List all claim files and sub-folders for a solution
    /// </summary>
    /// <param name="solutionId">unique identifier of solution</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <returns>list of BlobInfo - first item is folder for claim</returns>
    [HttpGet]
    [Route("EnumerateClaimFolderTree/{solutionId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<ClaimBlobInfoMap>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Solution not found in CRM")]
    public IActionResult EnumerateClaimFolderTree([FromRoute][Required]string solutionId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      return EnumerateClaimFolderTreeInternal(solutionId, pageIndex, pageSize);
    }
  }
}
