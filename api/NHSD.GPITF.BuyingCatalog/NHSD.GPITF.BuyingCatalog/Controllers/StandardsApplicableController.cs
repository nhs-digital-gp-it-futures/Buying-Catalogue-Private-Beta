using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Examples;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Manage claimed standards
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class StandardsApplicableController : Controller
  {
    private readonly IStandardsApplicableLogic _logic;

    /// <summary>
    /// constructor for ClaimedStandardController
    /// </summary>
    /// <param name="logic">business logic</param>
    public StandardsApplicableController(IStandardsApplicableLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Retrieve claim, given the claim’s CRM identifier
    /// </summary>
    /// <param name="id">CRM identifier of claim</param>
    /// <response code="200">Success</response>
    /// <response code="404">Claim not found in CRM</response>
    [HttpGet]
    [Route("{id}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(StandardsApplicable), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Claim not found in CRM")]
    public IActionResult ById([FromRoute][Required]string id)
    {
      var claim = _logic.ById(id);
      return claim != null ? (IActionResult)new OkObjectResult(claim) : new NotFoundResult();
    }

    /// <summary>
    /// Retrieve all claimed standards for a solution in a paged list,
    ///  given the solution’s CRM identifier
    /// </summary>
    /// <param name="solutionId">CRM identifier of solution</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    /// <response code="404">Solution not found in CRM</response>
    [HttpGet]
    [Route("BySolution/{solutionId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<StandardsApplicable>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Solution not found in CRM")]
    public IActionResult BySolution([FromRoute][Required]string solutionId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var stds = _logic.BySolution(solutionId);
      var retval = PaginatedList<StandardsApplicable>.Create(stds, pageIndex, pageSize);

      return new OkObjectResult(retval);
    }

    /// <summary>
    /// Create a new claimed standard for a solution
    /// </summary>
    /// <param name="claimedstandard">new claimed standard information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Solution not found in CRM</response>
    [HttpPost]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(StandardsApplicable), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Solution not found in CRM")]
    [SwaggerRequestExample(typeof(StandardsApplicable), typeof(StandardsApplicableExample), jsonConverter: typeof(StringEnumConverter))]
    public IActionResult Create([FromBody]StandardsApplicable claimedstandard)
    {
      try
      {
        var newStd = _logic.Create(claimedstandard);
        return new OkObjectResult(newStd);
      }
      catch (Exception ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }

    /// <summary>
    /// Update an existing claimed standard with new information
    /// </summary>
    /// <param name="claimedstandard">claimed standard with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Solution or ClaimedStandard not found in CRM</response>
    [HttpPut]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Solution or ClaimedStandard not found in CRM")]
    [SwaggerRequestExample(typeof(StandardsApplicable), typeof(StandardsApplicableExample), jsonConverter: typeof(StringEnumConverter))]
    public IActionResult Update([FromBody]StandardsApplicable claimedstandard)
    {
      try
      {
        _logic.Update(claimedstandard);
        return new OkResult();
      }
      catch (Exception ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }

    /// <summary>
    /// Delete an existing claimed standard for a solution
    /// </summary>
    /// <param name="claimedstandard">existing claimed standard information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Claimed standard not found in CRM</response>
    [HttpDelete]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "ClaimedStandard not found in CRM")]
    [SwaggerRequestExample(typeof(StandardsApplicable), typeof(StandardsApplicableExample), jsonConverter: typeof(StringEnumConverter))]
    public IActionResult Delete([FromBody]StandardsApplicable claimedstandard)
    {
      try
      {
        _logic.Delete(claimedstandard);
        return new OkResult();
      }
      catch (Exception ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }
  }
}
