using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class ValidatorBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<DummyValidatorBase>> _logger;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<DummyValidatorBase>>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyValidatorBase(_context.Object, _logger.Object));
    }

    [TestCase(Roles.Supplier)]
    public void MustBeSupplier_Supplier_Succeeds(string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new DummyValidatorBase(_context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase();

      validator.MustBeSupplier();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(Roles.Admin)]
    [TestCase(Roles.Buyer)]
    public void MustBeSupplier_NonSupplier_ReturnsError(string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new DummyValidatorBase(_context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase();

      validator.MustBeSupplier();
      var valres = validator.Validate(evidence);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Must be supplier")
        .And
        .HaveCount(1);
    }

    [TestCase(Roles.Admin)]
    public void MustBeAdmin_Admin_Succeeds(string role)
    {
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var validator = new DummyValidatorBase(_context.Object, _logger.Object);

      validator.MustBeAdmin();
      var valres = validator.Validate(_context.Object);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(Roles.Buyer)]
    [TestCase(Roles.Supplier)]
    public void MustBeAdmin_NonAdmin_ReturnsError(string role)
    {
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var validator = new DummyValidatorBase(_context.Object, _logger.Object);

      validator.MustBeAdmin();
      var valres = validator.Validate(_context.Object);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Must be admin")
        .And
        .HaveCount(1);
    }

    [TestCase(Roles.Admin)]
    [TestCase(Roles.Supplier)]
    public void MustBeAdminOrSupplier_AdminSupplier_Succeeds(string role)
    {
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var validator = new DummyValidatorBase(_context.Object, _logger.Object);

      validator.MustBeAdminOrSupplier();
      var valres = validator.Validate(_context.Object);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(Roles.Buyer)]
    public void MustBeAdminOrSupplier_NonAdminSupplier_ReturnsError(string role)
    {
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var validator = new DummyValidatorBase(_context.Object, _logger.Object);

      validator.MustBeAdminOrSupplier();
      var valres = validator.Validate(_context.Object);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Must be admin or supplier")
        .And
        .HaveCount(1);
    }
  }
}
