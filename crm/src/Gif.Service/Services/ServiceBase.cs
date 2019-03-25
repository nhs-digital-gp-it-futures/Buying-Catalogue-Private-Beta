#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Const;
using Gif.Service.Crm;
using System;
using System.Collections.Generic;
using System.Linq;
using Gif.Service.Contracts;

namespace Gif.Service.Services
{
  public abstract class ServiceBase<T> : IDatastoreBase<T>
  {
    protected IRepository Repository;

    public int? Count;

    public ServiceBase(IRepository repository)
    {
      Repository = repository;
    }

    public IEnumerable<T> GetPagingValues<T>(int? pageIndex, int? pageSize, IEnumerable<T> items, out int totalPages)
    {
      var skipPage = Paging.DefaultSkip;

      if (pageIndex != null && pageIndex != 1)
        skipPage = (int)pageIndex;

      var skipValue = pageSize ?? Paging.DefaultSkip;
      pageSize = pageSize ?? Paging.DefaultPageSize;
      totalPages = (int)Math.Ceiling(Convert.ToInt32(Count) / Convert.ToDecimal(pageSize));

      skipPage--;

      if (totalPages == 0 && items.Any())
        totalPages = 1;

      return items.Skip(skipPage * skipValue).Take((int)pageSize);
    }

    protected static List<T> GetInsertionTree<T>(List<T> allNodes) where T : IHasPreviousId
    {
      var roots = GetRoots(allNodes);
      var tree = new List<T>(roots);

      var next = GetChildren(roots, allNodes);
      while (next.Any())
      {
        tree.AddRange(next);
        next = GetChildren(next, allNodes);
      }

      return tree;
    }
    private static List<T> GetRoots<T>(List<T> allNodes) where T : IHasPreviousId
    {
      return allNodes.Where(x => x.PreviousId == null).ToList();
    }

    private static List<T> GetChildren<T>(List<T> parents, List<T> allNodes) where T : IHasPreviousId
    {
      return parents.SelectMany(parent => allNodes.Where(x => x.PreviousId == parent.Id)).ToList();
    }

  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
