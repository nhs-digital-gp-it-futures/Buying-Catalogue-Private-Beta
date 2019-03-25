using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers.Porcelain
{
  /// <summary>
  /// Access to an Extended Solution with its corresponding Technical Contacts, ClaimedCapability, ClaimedStandard et al
  /// </summary>
  [ApiVersion("1")]
  [ApiTag("porcelain")]
  [Route("api/porcelain/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class SolutionsExController : Controller
  {
    private readonly ISolutionsExLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public SolutionsExController(ISolutionsExLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Get a Solution with a list of corresponding TechnicalContact, ClaimedCapability, ClaimedStandard et al
    /// </summary>
    /// <param name="solutionId">CRM identifier of Solution</param>
    /// <response code="200">Success</response>
    [HttpGet]
    [Route("BySolution/{solutionId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(SolutionEx), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Solution not found in CRM")]
    public IActionResult BySolution([FromRoute][Required]string solutionId)
    {
      var solnEx = _logic.BySolution(solutionId);

      return solnEx?.Solution != null ? (IActionResult)new OkObjectResult(solnEx) : new NotFoundResult();
    }

    /// <summary>
    /// Update an existing Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al with new information
    /// </summary>
    /// <param name="solnEx">Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al not found in CRM</response>
    /// <response code="500">Datastore exception</response>
    [HttpPut]
    [Route("Update")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al not found in CRM")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.InternalServerError, description: "Datastore exception")]
    public IActionResult Update([FromBody]SolutionEx solnEx)
    {
      try
      {
        _logic.Update(solnEx);
        return new OkResult();
      }
      catch (ArgumentOutOfRangeException ex)
      {
        return new NotFoundObjectResult(ex);
      }
      catch (InvalidOperationException ex)
      {
        return new InternalServerErrorObjectResult(ex);
      }
      catch (Exception ex)
      {
        return new InternalServerErrorObjectResult(ex);
      }
    }

    /// <summary>
    /// Get a list of Solutions, each with a list of corresponding TechnicalContact, ClaimedCapability, ClaimedStandard et al
    /// </summary>
    /// <param name="organisationId">CRM identifier of Organisation</param>
    /// <response code="200">Success</response>
    [HttpGet]
    [Route("ByOrganisation/{organisationId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<SolutionEx>), description: "Success")]
    public IActionResult ByOrganisation([FromRoute][Required]string organisationId)
    {
      var solnExs = _logic.ByOrganisation(organisationId);

      return new OkObjectResult(solnExs);
    }
  }
}
