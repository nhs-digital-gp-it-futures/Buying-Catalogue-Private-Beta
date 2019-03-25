using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Create and retrieve StandardsApplicableEvidence
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class StandardsApplicableEvidenceController : Controller
  {
    private readonly IStandardsApplicableEvidenceLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public StandardsApplicableEvidenceController(IStandardsApplicableEvidenceLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Get all Evidence for the given Claim
    /// Each list is a distinct 'chain' of Evidence ie original Evidence with all subsequent Evidence
    /// The first item in each 'chain' is the most current Evidence.
    /// The last item in each 'chain' is the original Evidence.
    /// </summary>
    /// <param name="claimId">CRM identifier of Claim</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    /// <response code="404">Claim not found</response>
    [HttpGet]
    [Route("ByClaim/{claimId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<IEnumerable<StandardsApplicableEvidence>>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "StandardsApplicable not found")]
    public IActionResult ByClaim([FromRoute][Required]string claimId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var capEvidenc = _logic.ByClaim(claimId);
      var retval = PaginatedList<IEnumerable<StandardsApplicableEvidence>>.Create(capEvidenc, pageIndex, pageSize);

      return new OkObjectResult(capEvidenc);
    }

    /// <summary>
    /// Create a new evidence
    /// </summary>
    /// <param name="evidence">new evidence information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Claim not found</response>
    [HttpPost]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(StandardsApplicableEvidence), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Claim not found")]
    public IActionResult Create([FromBody]StandardsApplicableEvidence evidence)
    {
      try
      {
        var newEvidence = _logic.Create(evidence);
        return new OkObjectResult(newEvidence);
      }
      catch (FluentValidation.ValidationException ex)
      {
        return new InternalServerErrorObjectResult(ex);
      }
      catch (Exception ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }
  }
}
