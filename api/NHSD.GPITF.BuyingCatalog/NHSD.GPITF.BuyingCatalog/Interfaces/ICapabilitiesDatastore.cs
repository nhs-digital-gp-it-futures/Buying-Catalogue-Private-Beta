using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface ICapabilitiesDatastore
  {
    IEnumerable<Capabilities> ByFramework(string frameworkId);
    Capabilities ById(string id);
    IEnumerable<Capabilities> ByIds(IEnumerable<string> ids);
    IEnumerable<Capabilities> ByStandard(string standardId, bool isOptional);
    IEnumerable<Capabilities> GetAll();
  }
#pragma warning restore CS1591
}
