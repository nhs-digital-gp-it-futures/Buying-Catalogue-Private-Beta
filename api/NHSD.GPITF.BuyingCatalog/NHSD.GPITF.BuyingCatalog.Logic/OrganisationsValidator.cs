using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class OrganisationsValidator : ValidatorBase<Organisations>, IOrganisationsValidator
  {
    public OrganisationsValidator(IHttpContextAccessor context, ILogger<OrganisationsValidator> logger) :
      base(context, logger)
    {
    }
  }
}
