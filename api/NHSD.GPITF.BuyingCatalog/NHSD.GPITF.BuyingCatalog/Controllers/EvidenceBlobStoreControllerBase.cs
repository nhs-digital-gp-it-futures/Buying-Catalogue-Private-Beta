using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
#pragma warning disable CS1591
  public abstract class EvidenceBlobStoreControllerBase : Controller
  {
    protected readonly IEvidenceBlobStoreLogic _logic;

    public EvidenceBlobStoreControllerBase(IEvidenceBlobStoreLogic logic)
    {
      _logic = logic;
    }

    protected IActionResult AddEvidenceForClaimInternal(string claimId, IFormFile file, string filename, string subFolder = null)
    {
      try
      {
        var extUrl = _logic.AddEvidenceForClaim(claimId, file.OpenReadStream(), filename, subFolder);
        return new OkObjectResult(extUrl);
      }
      catch (FluentValidation.ValidationException ex)
      {
        return new InternalServerErrorObjectResult(ex);
      }
      catch (KeyNotFoundException ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }

    protected FileResult DownloadInternal(string claimId, string uniqueId)
    {
      try
      {
        var result = _logic.GetFileStream(claimId, uniqueId);
        var memory = new MemoryStream();

        result.FileStream.CopyTo(memory);
        memory.Position = 0;

        var retval = File(memory, result.ContentType, result.FileDownloadName);

        return retval;
      }
      catch (FluentValidation.ValidationException)
      {
        return null;
      }
      catch (KeyNotFoundException)
      {
        return null;
      }
    }

    protected IActionResult EnumerateFolderInternal(string claimId, string subFolder, int? pageIndex, int? pageSize)
    {
      try
      {
        var infos = _logic.EnumerateFolder(claimId, subFolder);
        var retval = PaginatedList<BlobInfo>.Create(infos, pageIndex, pageSize);
        return new OkObjectResult(retval);
      }
      catch (FluentValidation.ValidationException ex)
      {
        return new InternalServerErrorObjectResult(ex);
      }
      catch (KeyNotFoundException ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }

    protected IActionResult EnumerateClaimFolderTreeInternal(string solutionId, int? pageIndex, int? pageSize)
    {
      try
      {
        var infos = _logic.EnumerateClaimFolderTree(solutionId);
        var retval = PaginatedList<ClaimBlobInfoMap>.Create(infos, pageIndex, pageSize);
        return new OkObjectResult(retval);
      }
      catch (FluentValidation.ValidationException ex)
      {
        return new InternalServerErrorObjectResult(ex);
      }
      catch (KeyNotFoundException ex)
      {
        return new NotFoundObjectResult(ex);
      }
      catch (Exception ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }
  }
#pragma warning restore CS1591
}
