using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Logic;
using NUnit.Framework;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class CapabilityStandardDatastore_Tests : DatastoreBase_Tests<CapabilityStandardDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new CapabilityStandardDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config, _cache));
    }

    [Test]
    public void GetAll_ReturnsData()
    {
      var datastore = new CapabilityStandardDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<CapabilityStandardDatastore>>().Object, _policy, _config, _cache);

      var datas = datastore.GetAll().ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }
  }
}
