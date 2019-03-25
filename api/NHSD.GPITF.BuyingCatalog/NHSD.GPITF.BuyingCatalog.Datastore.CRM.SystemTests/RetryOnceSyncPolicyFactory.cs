using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using Polly;
using System;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  public sealed class RetryOnceSyncPolicyFactory : ISyncPolicyFactory
  {
    public ISyncPolicy Build(ILogger logger)
    {
      return Policy.Handle<Exception>().Retry();
    }
  }
}
