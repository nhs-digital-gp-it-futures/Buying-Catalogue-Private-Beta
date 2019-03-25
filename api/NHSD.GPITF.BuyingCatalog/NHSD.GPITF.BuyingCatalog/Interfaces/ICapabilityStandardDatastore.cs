using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface ICapabilityStandardDatastore
  {
    IEnumerable<CapabilityStandard> GetAll();
  }
#pragma warning restore CS1591
}
