using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class ContactsValidator : ValidatorBase<Contacts>, IContactsValidator
  {
    public ContactsValidator(IHttpContextAccessor context, ILogger<ContactsValidator> logger) :
      base(context, logger)
    {
    }
  }
}
