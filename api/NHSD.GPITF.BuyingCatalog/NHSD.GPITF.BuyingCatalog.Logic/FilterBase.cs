using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class FilterBase<T> : IFilter<T>
  {
    protected readonly IHttpContextAccessor _context;

    public FilterBase(IHttpContextAccessor context)
    {
      _context = context;
    }

    public abstract T Filter(T input);

    private T FilterInternal(T input)
    {
      if (input == null)
      {
        return input;
      }

      Verifier.Verify(input);

      return Filter(input);
    }

    public IEnumerable<T> Filter(IEnumerable<T> input)
    {
      return input.Select(x => FilterInternal(x)).Where(x => x != null);
    }
  }
}
