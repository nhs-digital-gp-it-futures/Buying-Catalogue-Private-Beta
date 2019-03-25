using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface ICapabilitiesLogic
  {
    IEnumerable<Capabilities> ByFramework(string frameworkId);
    Capabilities ById(string id);
    IEnumerable<Capabilities> ByIds(IEnumerable<string> ids);
    IEnumerable<Capabilities> ByStandard(string standardId, bool isOptional);
    IEnumerable<Capabilities> GetAll();
  }
#pragma warning restore CS1591
}
