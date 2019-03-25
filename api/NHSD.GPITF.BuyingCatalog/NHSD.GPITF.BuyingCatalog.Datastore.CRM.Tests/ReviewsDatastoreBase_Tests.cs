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
  public sealed class ReviewsDatastoreBase_Tests
  {
    private Mock<IRestClientFactory> _crmConnectionFactory;
    private Mock<ILogger<DummyReviewsDatastoreBase>> _logger;
    private Mock<ISyncPolicyFactory> _policy;
    private IConfiguration _config;

    [SetUp]
    public void SetUp()
    {
      _crmConnectionFactory = new Mock<IRestClientFactory>();
      _logger = new Mock<ILogger<DummyReviewsDatastoreBase>>();
      _policy = new Mock<ISyncPolicyFactory>();
      _config = new Mock<IConfiguration>().Object;
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyReviewsDatastoreBase(_crmConnectionFactory.Object, _logger.Object, _policy.Object, _config));
    }

    [Test]
    public void Class_Implements_Interface()
    {
      var obj = new DummyReviewsDatastoreBase(_crmConnectionFactory.Object, _logger.Object, _policy.Object, _config);

      var implInt = obj as IReviewsDatastore<ReviewsBase>;

      implInt.Should().NotBeNull();
    }
  }
}
