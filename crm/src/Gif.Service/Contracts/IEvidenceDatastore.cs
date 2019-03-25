using System;
using System.Collections.Generic;
using Gif.Service.Models;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IEvidenceDatastore<T> : IDatastoreBase<T>
  {
    IEnumerable<IEnumerable<T>> ByClaim(string claimId);

    IEnumerable<IEnumerable<T>> ByClaimMultiple(List<Guid> claimIds);
    T ById(string id);
    T Create(T evidence);
  }
#pragma warning restore CS1591
}
