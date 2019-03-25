using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IStandardsDatastore
  {
    IEnumerable<Standards> ByCapability(string capabilityId, bool isOptional);
    IEnumerable<Standards> ByFramework(string frameworkId);
    Standards ById(string id);
    IEnumerable<Standards> ByIds(IEnumerable<string> ids);
    IEnumerable<Standards> GetAll();
  }
#pragma warning restore CS1591
}
