using Gif.Service.Models;
using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IFrameworksDatastore : IDatastoreBase<Framework>
  {
    IEnumerable<Framework> ByCapability(string capabilityId);
    Framework ById(string id);
    IEnumerable<Framework> BySolution(string solutionId);
    IEnumerable<Framework> ByStandard(string standardId);
    IEnumerable<Framework> GetAll();
  }
#pragma warning restore CS1591
}
