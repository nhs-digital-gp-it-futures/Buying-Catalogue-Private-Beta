using FluentValidation;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IEvidenceValidator<T> : IValidatorBase<T> where T : EvidenceBase
  {
  }
}