using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.Tests
{
  [TestFixture]
  public sealed class EvidenceDatastoreBase_Tests
  {
    private Mock<IRestClientFactory> _crmConnectionFactory;
    private Mock<ILogger<EvidenceDatastoreBase<EvidenceBase>>> _logger;
    private Mock<ISyncPolicyFactory> _policy;
    private IConfiguration _config;

    [SetUp]
    public void SetUp()
    {
      _crmConnectionFactory = new Mock<IRestClientFactory>();
      _logger = new Mock<ILogger<EvidenceDatastoreBase<EvidenceBase>>>();
      _policy = new Mock<ISyncPolicyFactory>();
      _config = new Mock<IConfiguration>().Object;
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyEvidenceDatastoreBase(_crmConnectionFactory.Object, _logger.Object, _policy.Object, _config));
    }

    [Test]
    public void Class_Implements_Interface()
    {
      var obj = new DummyEvidenceDatastoreBase(_crmConnectionFactory.Object, _logger.Object, _policy.Object, _config);

      var implInt = obj as IEvidenceDatastore<EvidenceBase>;

      implInt.Should().NotBeNull();
    }
  }
}
