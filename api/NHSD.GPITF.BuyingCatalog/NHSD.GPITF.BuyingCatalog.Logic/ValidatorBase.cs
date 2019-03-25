using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class ValidatorBase<T> : AbstractValidator<T>, IValidatorBase<T>
  {
    protected readonly IHttpContextAccessor _context;
    private readonly ILogger<ValidatorBase<T>> _logger;

    public ValidatorBase(
      IHttpContextAccessor context,
      ILogger<ValidatorBase<T>> logger)
    {
      _context = context;
      _logger = logger;
    }

    public void MustBeAdmin()
    {
      RuleFor(x => x)
        .Must(x => _context.HasRole(Roles.Admin))
        .WithMessage("Must be admin");
    }

    public void MustBeSupplier()
    {
      RuleFor(x => x)
        .Must(x => _context.HasRole(Roles.Supplier))
        .WithMessage("Must be supplier");
    }

    public void MustBeAdminOrSupplier()
    {
      RuleFor(x => x)
        .Must(x =>
          _context.HasRole(Roles.Admin) ||
          _context.HasRole(Roles.Supplier))
        .WithMessage("Must be admin or supplier");
    }

    public void ValidateAndThrowEx(T instance, string ruleSet = null)
    {
      var result = this.Validate(instance, ruleSet: ruleSet);
      if (!result.IsValid)
      {
        var valex = new ValidationException(result.Errors);
        var msg = new { StackTrace = new StackTrace(true).ToString(), Exception = valex };
        _logger.LogError(JsonConvert.SerializeObject(msg));

        throw valex;
      }
    }
  }
}
