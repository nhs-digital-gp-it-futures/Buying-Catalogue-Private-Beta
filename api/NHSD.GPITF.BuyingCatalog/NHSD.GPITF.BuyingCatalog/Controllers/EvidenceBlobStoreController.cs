using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Manage evidence
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class EvidenceBlobStoreController : Controller
  {
    private readonly IEvidenceBlobStoreLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public EvidenceBlobStoreController(IEvidenceBlobStoreLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Create server side folder structure for specified solution
    /// </summary>
    /// <remarks>
    /// Server side folder structure is something like:
    /// --Organisation
    /// ----Solution
    /// ------Capability Evidence
    /// --------Appointment Management - Citizen
    /// --------Appointment Management - GP
    /// --------Clinical Decision Support
    /// --------[all other claimed capabilities]
    /// 
    /// Will be done automagically when solution status changes to SolutionStatus.CapabilitiesAssessment
    /// </remarks>
    /// <param name="solutionId">unique identifier of solution</param>
    /// <response code="200">Success</response>
    /// <response code="404">Solution not found in CRM</response>
    [HttpPut]
    [Route("PrepareForSolution/{solutionId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Solution not found in CRM")]
    public IActionResult PrepareForSolution([FromRoute][Required]string solutionId)
    {
      try
      {
        _logic.PrepareForSolution(solutionId);
        return new OkResult();
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
  }
}
