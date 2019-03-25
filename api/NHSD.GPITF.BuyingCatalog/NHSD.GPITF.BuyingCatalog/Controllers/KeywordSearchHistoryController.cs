using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers
{
  /// <summary>
  /// Get keywords buyers have been using to search for solutions
  /// </summary>
  [ApiVersion("1")]
  [ApiTag("porcelain")]
  [Route("api/porcelain/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class KeywordSearchHistoryController : Controller
  {
    private readonly IKeywordSearchHistoryLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public KeywordSearchHistoryController(IKeywordSearchHistoryLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Get keywords and how many times they occurred in a specified UTC date range
    /// </summary>
    /// <param name="startDate">start of UTC date range eg 1965-05-15
    /// Defaults to 10 years ago</param>
    /// <param name="endDate">end of UTC date range eg 2006-02-20
    /// Defaults to 10 years from now</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    [HttpGet]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<KeywordCount>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "No keywords found")]
    public IActionResult Get([FromQuery]DateTime? startDate, [FromQuery]DateTime? endDate, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      try
      {
      var logs = _logic.Get(startDate ?? DateTime.Now.AddYears(-10), endDate ?? DateTime.Now.AddYears(10));
      var retval = PaginatedList<KeywordCount>.Create(logs, pageIndex, pageSize);
      return logs.Count() > 0 ? (IActionResult)new OkObjectResult(retval) : new NotFoundResult();
      }
      catch (Exception)
      {
        return new ForbidResult();
      }
    }
  }
}
