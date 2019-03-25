using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain
{
#pragma warning disable CS1591
  public interface ISolutionsExLogic
  {
    SolutionEx BySolution(string solutionId);
    void Update(SolutionEx solnEx);
    IEnumerable<SolutionEx> ByOrganisation(string organisationId);
  }
#pragma warning restore CS1591
}
