using System;
using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IReviewsDatastore<T> : IDatastoreBase<T>
  {
    IEnumerable<IEnumerable<T>> ByEvidence(string evidenceId);

    IEnumerable<IEnumerable<T>> ByEvidenceMultiple(List<Guid> evidenceIds);
    T ById(string id);
    T Create(T review);
    void Delete(T review);
  }
#pragma warning restore CS1591
}
