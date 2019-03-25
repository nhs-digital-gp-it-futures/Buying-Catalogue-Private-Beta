using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IFilter<T>
  {
    IEnumerable<T> Filter(IEnumerable<T> input);
  }
}
