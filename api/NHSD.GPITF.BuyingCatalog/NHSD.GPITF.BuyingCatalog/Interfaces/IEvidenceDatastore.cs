using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IEvidenceDatastore<T>
  {
    IEnumerable<IEnumerable<T>> ByClaim(string claimId);
    T ById(string id);
    T Create(T evidence);
  }
#pragma warning restore CS1591
}
