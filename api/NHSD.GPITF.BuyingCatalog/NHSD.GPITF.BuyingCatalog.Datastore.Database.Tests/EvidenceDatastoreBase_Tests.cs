using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database.Tests
{
  [TestFixture]
  public sealed class EvidenceDatastoreBase_Tests
  {
    private Mock<IDbConnectionFactory> _dbConnectionFactory;
    private Mock<ILogger<DummyEvidenceDatastoreBase>> _logger;
    private Mock<ISyncPolicyFactory> _policy;

    [SetUp]
    public void SetUp()
    {
      _dbConnectionFactory = new Mock<IDbConnectionFactory>();
      _logger = new Mock<ILogger<DummyEvidenceDatastoreBase>>();
      _policy = new Mock<ISyncPolicyFactory>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyEvidenceDatastoreBase(_dbConnectionFactory.Object, _logger.Object, _policy.Object));
    }

    [Test]
    public void Class_Implements_Interface()
    {
      var obj = new DummyEvidenceDatastoreBase(_dbConnectionFactory.Object, _logger.Object, _policy.Object);

      var implInt = obj as IEvidenceDatastore<EvidenceBase>;

      implInt.Should().NotBeNull();
    }
  }
}
