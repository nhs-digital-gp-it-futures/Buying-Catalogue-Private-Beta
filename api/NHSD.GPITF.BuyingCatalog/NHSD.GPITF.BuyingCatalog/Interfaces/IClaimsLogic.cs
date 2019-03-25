using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IClaimsLogic<T>
  {
    T ById(string id);
    IEnumerable<T> BySolution(string solutionId);
    T Create(T claim);
    void Update(T claim);
    void Delete(T claim);
  }
#pragma warning restore CS1591
}
