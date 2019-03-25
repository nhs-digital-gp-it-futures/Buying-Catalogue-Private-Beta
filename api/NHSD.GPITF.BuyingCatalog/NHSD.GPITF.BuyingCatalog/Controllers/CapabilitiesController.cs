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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Manage capabilities
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class CapabilitiesController : Controller
  {
    private readonly ICapabilitiesLogic _logic;

    /// <summary>
    /// constructor for CapabilityController
    /// </summary>
    /// <param name="logic">business logic</param>
    public CapabilitiesController(ICapabilitiesLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Get existing Capability/s which are in the given Framework
    /// </summary>
    /// <param name="frameworkId">CRM identifier of Framework</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    /// <response code="404">Framework not found in CRM</response>
    [HttpGet]
    [Route("ByFramework/{frameworkId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<Capabilities>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Framework not found in CRM")]
    public IActionResult ByFramework([FromRoute][Required]string frameworkId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var caps = _logic.ByFramework(frameworkId);
      var retval = PaginatedList<Capabilities>.Create(caps, pageIndex, pageSize);

      return new OkObjectResult(retval);
    }

    /// <summary>
    /// Get an existing capability given its CRM identifier
    /// Typically used to retrieve previous version
    /// </summary>
    /// <param name="id">CRM identifier of capability to find</param>
    /// <response code="200">Success</response>
    /// <response code="404">Capability not found in CRM</response>
    [HttpGet]
    [Route("ById/{id}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(Capabilities), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "No capabilities not found in CRM")]
    public IActionResult ById([FromRoute][Required]string id)
    {
      var cap = _logic.ById(id);
      return cap != null ? (IActionResult)new OkObjectResult(cap) : new NotFoundResult();
    }

    /// <summary>
    /// Get several existing capabilities given their CRM identifiers
    /// </summary>
    /// <param name="ids">Array of CRM identifiers of capabilities to find</param>
    /// <response code="200">Success</response>
    /// <response code="404">Capabilities not found in CRM</response>
    [HttpPost]
    [Route("ByIds")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Capabilities>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Capabilities not found in CRM")]
    [SwaggerRequestExample(typeof(IEnumerable<string>), typeof(CapabilityIdsExample), jsonConverter: typeof(StringEnumConverter))]
    public IActionResult ByIds([FromBody][Required]IEnumerable<string> ids)
    {
      var caps = _logic.ByIds(ids);

      return new OkObjectResult(caps);
    }

    /// <summary>
    /// Get existing Capability/s which require the given/optional Standard
    /// </summary>
    /// <param name="standardId">CRM identifier of Standard</param>
    /// <param name="isOptional">true if the specified Standard is optional with the Capability</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    /// <response code="404">Standard not found in CRM</response>
    [HttpGet]
    [Route("ByStandard/{standardId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<Capabilities>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Capability not found in CRM")]
    public IActionResult ByStandard([FromRoute][Required]string standardId, [FromQuery][Required]bool isOptional, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var caps = _logic.ByStandard(standardId, isOptional);
      var retval = PaginatedList<Capabilities>.Create(caps, pageIndex, pageSize);

      return new OkObjectResult(retval);
    }

    /// <summary>
    /// Retrieve all current capabilities in a paged list
    /// </summary>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success - if no capabilities found, return empty list</response>
    [HttpGet]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<Capabilities>), description: "Success - if no capabilities found, return empty list")]
    public IActionResult Get([FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var allCaps = _logic.GetAll();
      var retval = PaginatedList<Capabilities>.Create(allCaps, pageIndex, pageSize);
      return new OkObjectResult(retval);
    }
  }
}
