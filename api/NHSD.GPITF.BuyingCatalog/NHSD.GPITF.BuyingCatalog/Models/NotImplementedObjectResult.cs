using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  public sealed class NotImplementedObjectResult : ObjectResult
  {
    public NotImplementedObjectResult(object value) :
      base(value)
    {
      StatusCode = (int)HttpStatusCode.NotImplemented;
    }
  }
#pragma warning restore CS1591
}
