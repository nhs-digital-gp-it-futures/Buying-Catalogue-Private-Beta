using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Find and retrieve organisations
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class OrganisationsController : Controller
  {
    private readonly IOrganisationsLogic _logic;

    /// <summary>
    /// constructor for OrganisationController
    /// </summary>
    /// <param name="logic">business logic</param>
    public OrganisationsController(IOrganisationsLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Retrieve an Organisation for the given Contact
    /// </summary>
    /// <param name="contactId">CRM identifier of Contact</param>
    /// <response code="200">Success</response>
    /// <response code="404">Organisation not found</response>
    [HttpGet]
    [Route("ByContact/{contactId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(Organisations), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Organisation not found")]
    public IActionResult ByContact([FromRoute][Required]string contactId)
    {
      var org = _logic.ByContact(contactId);
      return org != null ? (IActionResult)new OkObjectResult(org) : new NotFoundResult();
    }
  }
}
