using Gif.Service.Models;
using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface ISolutionsDatastore : IDatastoreBase<Solution>
  {
    IEnumerable<Solution> ByFramework(string frameworkId);
    Solution ById(string id);
    IEnumerable<Solution> ByOrganisation(string organisationId);
    Solution Create(Solution solution);
    void Update(Solution solution);
    void Delete(Solution solution);
  }
#pragma warning restore CS1591
}
