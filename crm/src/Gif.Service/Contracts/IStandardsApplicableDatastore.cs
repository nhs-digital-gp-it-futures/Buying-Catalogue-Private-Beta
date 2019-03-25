using Gif.Service.Models;
using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IStandardsApplicableDatastore : IDatastoreBase<StandardApplicable>
  {
    StandardApplicable ById(string id);
    IEnumerable<StandardApplicable> BySolution(string solutionId);
    StandardApplicable Create(StandardApplicable standardApplicable);
    void Update(StandardApplicable standardApplicable);
    void Delete(StandardApplicable standardApplicable);
  }
#pragma warning restore CS1591
}
