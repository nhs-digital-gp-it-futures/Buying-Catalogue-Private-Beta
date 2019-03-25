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
  public sealed class ClaimsDatastoreBase_Tests
  {
    private Mock<IDbConnectionFactory> _dbConnectionFactory;
    private Mock<ILogger<DummyClaimsDatastoreBase>> _logger;
    private Mock<ISyncPolicyFactory> _policy;

    [SetUp]
    public void SetUp()
    {
      _dbConnectionFactory = new Mock<IDbConnectionFactory>();
      _logger = new Mock<ILogger<DummyClaimsDatastoreBase>>();
      _policy = new Mock<ISyncPolicyFactory>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyClaimsDatastoreBase(_dbConnectionFactory.Object, _logger.Object, _policy.Object));
    }

    [Test]
    public void Class_Implements_Interface()
    {
      var obj = new DummyClaimsDatastoreBase(_dbConnectionFactory.Object, _logger.Object, _policy.Object);

      var implInt = obj as IClaimsDatastore<ClaimsBase>;

      implInt.Should().NotBeNull();
    }
  }
}
