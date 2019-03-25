using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  public sealed class InternalServerErrorObjectResult : ObjectResult
  {
    public InternalServerErrorObjectResult(object value) :
      base(value)
    {
      StatusCode = (int)HttpStatusCode.InternalServerError;
    }
  }
#pragma warning restore CS1591
}
