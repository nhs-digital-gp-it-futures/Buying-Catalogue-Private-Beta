using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Manage links between Solution and Framework
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class LinkManagerController : Controller
  {
    private readonly ILinkManagerLogic _logic;

    /// <summary>
    /// constructor for LinkManagerController
    /// </summary>
    /// <param name="logic">business logic</param>
    public LinkManagerController(ILinkManagerLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Create a link between a Framework and a Solution
    /// </summary>
    /// <param name="frameworkId">CRM identifier of Framework</param>
    /// <param name="solutionId">CRM identifier of Solution</param>
    /// <response code="200">Success</response>
    /// <response code="404">Framework or Solution not found in CRM</response>
    [HttpPost]
    [Route("FrameworkSolution/Create/{frameworkId}/{solutionId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "One entity not found or link already exists")]
    public IActionResult FrameworkSolutionCreate([FromRoute][Required]string frameworkId, [FromRoute][Required]string solutionId)
    {
      try
      {
        _logic.FrameworkSolutionCreate(frameworkId, solutionId);
        return new OkResult();
      }
      catch (Exception ex)
      {
        return new NotFoundObjectResult(ex);
      }
    }
  }
}
