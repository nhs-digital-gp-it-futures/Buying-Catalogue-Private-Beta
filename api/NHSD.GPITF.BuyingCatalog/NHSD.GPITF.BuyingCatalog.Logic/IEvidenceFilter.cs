using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public interface IEvidenceFilter<T> : IFilter<T> where T : IEnumerable<EvidenceBase>
  {
  }
}