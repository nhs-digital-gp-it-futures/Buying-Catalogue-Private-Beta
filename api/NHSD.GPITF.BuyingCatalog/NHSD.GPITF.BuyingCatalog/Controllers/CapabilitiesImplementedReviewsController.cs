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
  /// Create and retrieve CapabilitiesImplementedReviews
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.Buyer + "," + Roles.Supplier,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class CapabilitiesImplementedReviewsController : Controller
  {
    private readonly ICapabilitiesImplementedReviewsLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public CapabilitiesImplementedReviewsController(ICapabilitiesImplementedReviewsLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Get all Reviews for a CapabilitiesImplemented
    /// Each list is a distinct 'chain' of Review ie original Review with all subsequent Review
    /// The first item in each 'chain' is the most current Review.
    /// The last item in each 'chain' is the original Review.
    /// </summary>
    /// <param name="evidenceId">CRM identifier of CapabilitiesImplementedEvidence</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    /// <response code="404">CapabilitiesImplementedEvidence not found</response>
    [HttpGet]
    [Route("ByEvidence/{evidenceId}")]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<IEnumerable<CapabilitiesImplementedReviews>>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Evidence not found")]
    public IActionResult ByEvidence([FromRoute][Required]string evidenceId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var reviews = _logic.ByEvidence(evidenceId);
      var retval = PaginatedList<IEnumerable<CapabilitiesImplementedReviews>>.Create(reviews, pageIndex, pageSize);
      return reviews.Count() > 0 ? (IActionResult)new OkObjectResult(reviews) : new NotFoundResult();
    }

    /// <summary>
    /// Create a new Review for a CapabilitiesImplemented
    /// </summary>
    /// <param name="review">new Review information</param>
    /// <response code="200">Success</response>
    /// <response code="404">CapabilitiesImplementedEvidence not found</response>
    [HttpPost]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(CapabilitiesImplementedReviews), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "CapabilitiesImplemented not found")]
    public IActionResult Create([FromBody]CapabilitiesImplementedReviews review)
    {
      try
      {
        var newReview = _logic.Create(review);
        return new OkObjectResult(newReview);
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
