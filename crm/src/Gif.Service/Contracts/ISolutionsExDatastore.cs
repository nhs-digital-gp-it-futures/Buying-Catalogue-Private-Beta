using Gif.Service.Models;
using System.Collections.Generic;

//using NHSD.GPITF.BuyingCatalog.Models.Porcelain;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface ISolutionsExDatastore
  {
    SolutionEx BySolution(string solutionId);
    void Update(SolutionEx solnEx);
    IEnumerable<SolutionEx> ByOrganisation(string organisationId);
  }
#pragma warning restore CS1591
}
