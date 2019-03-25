using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  public abstract class DatastoreBase_Tests<T>
  {
    protected ILogger<T> _logger = new Mock<ILogger<T>>().Object;
    protected ISyncPolicyFactory _policy = new RetryOnceSyncPolicyFactory();
    protected IConfiguration _config = new Mock<IConfiguration>().Object;
    protected IDatastoreCache _cache = new Mock<IDatastoreCache>().Object;
  }
}
