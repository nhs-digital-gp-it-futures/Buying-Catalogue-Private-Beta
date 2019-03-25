using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IFrameworksDatastore
  {
    IEnumerable<Frameworks> ByCapability(string capabilityId);
    Frameworks ById(string id);
    IEnumerable<Frameworks> BySolution(string solutionId);
    IEnumerable<Frameworks> ByStandard(string standardId);
    IEnumerable<Frameworks> GetAll();
  }
#pragma warning restore CS1591
}
