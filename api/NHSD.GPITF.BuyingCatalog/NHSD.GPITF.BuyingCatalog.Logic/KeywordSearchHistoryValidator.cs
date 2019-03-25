using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class KeywordSearchHistoryValidator : ValidatorBase<object>, IKeywordSearchHistoryValidator
  {
    public KeywordSearchHistoryValidator(IHttpContextAccessor context, ILogger<KeywordSearchHistoryValidator> logger) :
      base(context, logger)
    {
      MustBeAdminOrSupplier();
    }
  }
}
