using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface ISolutionsModifier
  {
    void ForCreate(Solutions input);
    void ForUpdate(Solutions input);
  }
}
