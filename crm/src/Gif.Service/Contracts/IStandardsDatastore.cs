using Gif.Service.Models;
using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IStandardsDatastore : IDatastoreBase<Standard>
  {
    IEnumerable<Standard> ByCapability(string capabilityId, bool? isOptional);
    IEnumerable<Standard> ByFramework(string frameworkId);
    Standard ById(string id);
    IEnumerable<Standard> ByIds(IEnumerable<string> ids);
    IEnumerable<Standard> GetAll();
  }
#pragma warning restore CS1591
}
