using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IEvidenceBaseModifier<T> where T : EvidenceBase
  {
    void ForCreate(T input);
    void ForUpdate(T input);
  }
}
