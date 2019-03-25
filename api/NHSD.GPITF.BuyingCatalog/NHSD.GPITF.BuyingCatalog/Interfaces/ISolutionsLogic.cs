using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface ISolutionsLogic
  {
    IEnumerable<Solutions> ByFramework(string frameworkId);
    Solutions ById(string id);
    IEnumerable<Solutions> ByOrganisation(string organisationId);
    Solutions Create(Solutions solution);
    void Update(Solutions solution);
    void Delete(Solutions solution);
  }
#pragma warning restore CS1591
}
