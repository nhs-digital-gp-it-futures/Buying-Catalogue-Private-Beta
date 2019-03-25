using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint.Tests
{
  [TestFixture]
  public sealed class EvidenceBlobStoreValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<EvidenceBlobStoreValidator>> _logger;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<EvidenceBlobStoreValidator>>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new EvidenceBlobStoreValidator(_context.Object, _logger.Object));
    }

    [TestCase(Roles.Admin)]
    [TestCase(Roles.Supplier)]
    public void MustBeAdminOrSupplier_AdminSupplier_Succeeds(string role)
    {
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var validator = new EvidenceBlobStoreValidator(_context.Object, _logger.Object);

      validator.MustBeAdminOrSupplier();
      var valres = validator.Validate(role);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(Roles.Buyer)]
    public void MustBeAdminOrSupplier_NonAdminSupplier_ReturnsError(string role)
    {
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var validator = new EvidenceBlobStoreValidator(_context.Object, _logger.Object);

      validator.MustBeAdminOrSupplier();
      var valres = validator.Validate(role);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Must be admin or supplier")
        .And
        .HaveCount(1);
    }
  }
}
