using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class CapabilitiesImplementedValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<CapabilitiesImplementedValidator>> _logger;
    private Mock<ICapabilitiesImplementedDatastore> _claimDatastore;
    private Mock<IContactsDatastore> _contactsDatastore;
    private Mock<ISolutionsDatastore> _solutionsDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<CapabilitiesImplementedValidator>>();
      _claimDatastore = new Mock<ICapabilitiesImplementedDatastore>();
      _contactsDatastore = new Mock<IContactsDatastore>();
      _solutionsDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new CapabilitiesImplementedValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object));
    }

    [TestCase(Roles.Supplier)]
    public void MustBePending_Draft_Succeeds(string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new CapabilitiesImplementedValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetCapabilitiesImplemented(status: CapabilitiesImplementedStatus.Draft);

      validator.MustBePending();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(Roles.Buyer)]
    [TestCase(Roles.Admin)]
    public void MustBePending_Draft_ReturnsError(string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new CapabilitiesImplementedValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetCapabilitiesImplemented(status: CapabilitiesImplementedStatus.Draft);

      validator.MustBePending();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Only supplier can delete a draft claim")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBePending_NonDraft_ReturnsError(
      [Values(
        Roles.Buyer,
        Roles.Supplier,
        Roles.Admin
      )]
        string role,
      [Values(
        CapabilitiesImplementedStatus.Submitted,
        CapabilitiesImplementedStatus.Remediation,
        CapabilitiesImplementedStatus.Approved,
        CapabilitiesImplementedStatus.Rejected
        )]
        CapabilitiesImplementedStatus status)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new CapabilitiesImplementedValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetCapabilitiesImplemented(status: status);

      validator.MustBePending();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Only supplier can delete a draft claim")
        .And
        .HaveCount(1);
    }

    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Draft, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Submitted, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Remediation, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Submitted, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Approved, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Rejected, Roles.Admin)]
    public void MustBeValidStatusTransition_Valid_Succeeds(CapabilitiesImplementedStatus oldStatus, CapabilitiesImplementedStatus newStatus, string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new CapabilitiesImplementedValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var oldClaim = Creator.GetCapabilitiesImplemented(status: oldStatus);
      var newClaim = Creator.GetCapabilitiesImplemented(status: newStatus);
      _claimDatastore.Setup(x => x.ById(newClaim.Id)).Returns(oldClaim);

      validator.MustBeValidStatusTransition();
      var valres = validator.Validate(newClaim);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Draft, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Draft, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Submitted, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Submitted, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Remediation, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Remediation, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Remediation, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Approved, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Approved, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Approved, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Rejected, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Rejected, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Rejected, Roles.Supplier)]

    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Remediation, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Remediation, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Draft, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Draft, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Draft, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Submitted, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Submitted, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Submitted, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Approved, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Approved, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Rejected, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Rejected, Roles.Supplier)]

    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Submitted, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Submitted, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Draft, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Draft, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Draft, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Remediation, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Remediation, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Remediation, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Approved, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Approved, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Approved, Roles.Supplier)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Rejected, Roles.Admin)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Rejected, Roles.Buyer)]
    [TestCase(CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Rejected, Roles.Supplier)]
    public void MustBeValidStatusTransition_Invalid_ReturnsError(CapabilitiesImplementedStatus oldStatus, CapabilitiesImplementedStatus newStatus, string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new CapabilitiesImplementedValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var oldClaim = Creator.GetCapabilitiesImplemented(status: oldStatus);
      var newClaim = Creator.GetCapabilitiesImplemented(status: newStatus);
      _claimDatastore.Setup(x => x.ById(newClaim.Id)).Returns(oldClaim);

      validator.MustBeValidStatusTransition();
      var valres = validator.Validate(newClaim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid Status transition")
        .And
        .HaveCount(1);
    }

    [Test]
    public void Validate_Update_FinalState_ReturnsError(
      [Values(
        CapabilitiesImplementedStatus.Approved,
        CapabilitiesImplementedStatus.Rejected)]
          CapabilitiesImplementedStatus oldStatus,
      [Values(
        CapabilitiesImplementedStatus.Draft,
        CapabilitiesImplementedStatus.Submitted,
        CapabilitiesImplementedStatus.Remediation,
        CapabilitiesImplementedStatus.Approved,
        CapabilitiesImplementedStatus.Rejected)]
          CapabilitiesImplementedStatus newStatus,
      [Values(
        Roles.Admin,
        Roles.Buyer,
        Roles.Supplier)]
          string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new CapabilitiesImplementedValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var oldClaim = Creator.GetCapabilitiesImplemented(status: oldStatus);
      var newClaim = Creator.GetCapabilitiesImplemented(status: newStatus);
      _claimDatastore.Setup(x => x.ById(newClaim.Id)).Returns(oldClaim);

      validator.MustBeValidStatusTransition();
      var valres = validator.Validate(newClaim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid Status transition")
        .And
        .HaveCount(1);
    }
  }
}
