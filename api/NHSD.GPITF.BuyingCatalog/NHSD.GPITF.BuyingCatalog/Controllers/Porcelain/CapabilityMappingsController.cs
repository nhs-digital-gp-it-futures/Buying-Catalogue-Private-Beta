using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers.Porcelain
{
  /// <summary>
  /// Access to capabilities with a list of corresponding, optional standards
  /// </summary>
  [ApiVersion("1")]
  [ApiTag("porcelain")]
  [Route("api/porcelain/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class CapabilityMappingsController : Controller
  {
    private readonly ICapabilityMappingsLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public CapabilityMappingsController(ICapabilityMappingsLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Get capabilities with a list of corresponding, optional standards
    /// </summary>
    /// <response code="200">Success</response>
    [HttpGet]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(CapabilityMappings), description: "Success")]
    public IActionResult Get()
    {
      var capMaps = _logic.GetAll();
      return new OkObjectResult(capMaps);
    }
  }
}
