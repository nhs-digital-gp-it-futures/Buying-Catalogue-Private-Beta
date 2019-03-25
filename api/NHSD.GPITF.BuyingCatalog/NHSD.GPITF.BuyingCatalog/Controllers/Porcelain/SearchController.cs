using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Attributes;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace NHSD.GPITF.BuyingCatalog.Controllers.Porcelain
{
  /// <summary>
  /// Find solutions in the system
  /// </summary>
  [ApiVersion("1")]
  [ApiTag("porcelain")]
  [Route("api/porcelain/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class SearchController : Controller
  {
    private readonly ISearchLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public SearchController(ISearchLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Get existing solution/s which are related to the given keyword <br />
    /// Keyword is not case sensitive <br />
    /// Capabilities are searched for capabilities which contain
    /// the keyword in the capability name or descriptions.  This
    /// forms a set of desired capabilities. <br />
    /// These desired capabilities are then matched to solution/s which
    /// implement at least one of the desired capabilities. <br />
    /// An 'ideal' solution would be one which only implements all
    /// of the desired capabilities. <br />
    /// Each solution is given a 'distance' which represents how many
    /// different capabilites the solution implements, compared to the
    /// set of desired capabilities: <br />
    ///   zero     == solution has exactly capabilities desired <br />
    ///   positive == solution has more capabilities than desired <br />
    ///   negative == solution has less capabilities than desired <br />
    /// </summary>
    /// <param name="keyword">keyword describing a solution or capability</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    [HttpGet]
    [Route("ByKeyword/{keyword}")]
    [ValidateModelState]
    [AllowAnonymous]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<SearchResult>), description: "Success")]
    public IActionResult ByKeyword([FromRoute][Required]string keyword, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var solutions = _logic.ByKeyword(keyword);
      var retval = PaginatedList<SearchResult>.Create(solutions, pageIndex, pageSize);
      return new OkObjectResult(retval);
    }
  }
}
