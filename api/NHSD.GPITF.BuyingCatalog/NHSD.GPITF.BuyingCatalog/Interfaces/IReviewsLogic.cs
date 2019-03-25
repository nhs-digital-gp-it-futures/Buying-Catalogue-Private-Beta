using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IReviewsLogic<T>
  {
    IEnumerable<IEnumerable<T>> ByEvidence(string evidenceId);
    T Create(T review);
  }
#pragma warning restore CS1591
}