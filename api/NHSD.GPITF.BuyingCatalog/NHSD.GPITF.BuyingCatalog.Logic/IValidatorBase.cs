using FluentValidation;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IValidatorBase<T> : IValidator<T>
  {
    void ValidateAndThrowEx(T instance, string ruleSet = null);
  }
}
