using Microsoft.Extensions.Logging;
using Polly;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface ISyncPolicyFactory
  {
    ISyncPolicy Build(ILogger logger);
  }
#pragma warning restore CS1591
}
