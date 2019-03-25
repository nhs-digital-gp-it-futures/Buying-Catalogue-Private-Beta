using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface IDatastoreBase<T>
  {
    IEnumerable<T> GetPagingValues<T>(int? pageIndex, int? pageSize, IEnumerable<T> items, out int totalPages);
  }
#pragma warning restore CS1591
}
