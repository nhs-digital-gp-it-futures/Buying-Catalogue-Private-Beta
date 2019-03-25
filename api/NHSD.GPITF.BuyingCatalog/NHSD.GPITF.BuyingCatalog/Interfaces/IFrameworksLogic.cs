using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IFrameworksLogic
  {
    IEnumerable<Frameworks> GetAll();
    IEnumerable<Frameworks> BySolution(string solutionId);
    IEnumerable<Frameworks> ByStandard(string standardId);
    Frameworks ById(string id);
    IEnumerable<Frameworks> ByCapability(string capabilityId);
  }
#pragma warning restore CS1591
}
