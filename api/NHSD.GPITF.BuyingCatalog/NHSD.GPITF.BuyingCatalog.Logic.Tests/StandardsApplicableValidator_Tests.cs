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
  public sealed class StandardsApplicableValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<StandardsApplicableValidator>> _logger;
    private Mock<IStandardsApplicableDatastore> _claimDatastore;
    private Mock<IContactsDatastore> _contactsDatastore;
    private Mock<ISolutionsDatastore> _solutionsDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<StandardsApplicableValidator>>();
      _claimDatastore = new Mock<IStandardsApplicableDatastore>();
      _contactsDatastore = new Mock<IContactsDatastore>();
      _solutionsDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new StandardsApplicableValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object));
    }

    [Test]
    public void MustBePending_Draft_Succeeds(
      [Values(Roles.Supplier)]
        string role,
      [Values(StandardsApplicableStatus.Draft, StandardsApplicableStatus.NotStarted)]
        StandardsApplicableStatus status)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new StandardsApplicableValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetStandardsApplicable(status: status);

      validator.MustBePending();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBePending_Draft_ReturnsError(
      [Values(Roles.Admin, Roles.Buyer)]
        string role,
      [Values(StandardsApplicableStatus.Draft, StandardsApplicableStatus.NotStarted)]
        StandardsApplicableStatus status)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new StandardsApplicableValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetStandardsApplicable(status: status);

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
        StandardsApplicableStatus.Submitted,
        StandardsApplicableStatus.Remediation,
        StandardsApplicableStatus.Approved,
        StandardsApplicableStatus.ApprovedFirstOfType,
        StandardsApplicableStatus.ApprovedPartial,
        StandardsApplicableStatus.Rejected
        )]
        StandardsApplicableStatus status)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new StandardsApplicableValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetStandardsApplicable(status: status);

      validator.MustBePending();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Only supplier can delete a draft claim")
        .And
        .HaveCount(1);
    }

    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Draft, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Submitted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Remediation, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Submitted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Rejected, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Approved, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedPartial, Roles.Admin)]
    public void MustBeValidStatusTransition_Valid_Succeeds(StandardsApplicableStatus oldStatus, StandardsApplicableStatus newStatus, string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new StandardsApplicableValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var oldClaim = Creator.GetStandardsApplicable(status: oldStatus);
      var newClaim = Creator.GetStandardsApplicable(status: newStatus);
      _claimDatastore.Setup(x => x.ById(newClaim.Id)).Returns(oldClaim);

      validator.MustBeValidStatusTransition();
      var valres = validator.Validate(newClaim);

      valres.Errors.Should().BeEmpty();
    }


    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Draft, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Draft, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.NotStarted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.NotStarted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.NotStarted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Submitted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Submitted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Submitted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Remediation, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Remediation, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Remediation, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Approved, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Approved, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Approved, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.ApprovedPartial, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.ApprovedPartial, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.ApprovedPartial, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Rejected, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Rejected, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Rejected, Roles.Supplier)]

    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Submitted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Submitted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.NotStarted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.NotStarted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.NotStarted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Remediation, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Remediation, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Remediation, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Approved, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Approved, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Approved, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.ApprovedPartial, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.ApprovedPartial, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.ApprovedPartial, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Rejected, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Rejected, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Draft, StandardsApplicableStatus.Rejected, Roles.Supplier)]

    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Remediation, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Remediation, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Draft, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Draft, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Draft, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Submitted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Submitted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Submitted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.NotStarted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.NotStarted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.NotStarted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Approved, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Approved, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedPartial, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedPartial, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Rejected, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Rejected, Roles.Supplier)]

    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Submitted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Submitted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.NotStarted, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.NotStarted, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.NotStarted, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Draft, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Draft, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Draft, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Remediation, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Remediation, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Remediation, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Approved, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Approved, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Approved, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.ApprovedFirstOfType, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.ApprovedPartial, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.ApprovedPartial, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.ApprovedPartial, Roles.Supplier)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Rejected, Roles.Admin)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Rejected, Roles.Buyer)]
    [TestCase(StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Rejected, Roles.Supplier)]
    public void MustBeValidStatusTransition_Invalid_ReturnsError(StandardsApplicableStatus oldStatus, StandardsApplicableStatus newStatus, string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new StandardsApplicableValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var oldClaim = Creator.GetStandardsApplicable(status: oldStatus);
      var newClaim = Creator.GetStandardsApplicable(status: newStatus);
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
        StandardsApplicableStatus.Approved,
        StandardsApplicableStatus.ApprovedPartial,
        StandardsApplicableStatus.ApprovedFirstOfType,
        StandardsApplicableStatus.Rejected)]
          StandardsApplicableStatus oldStatus,
      [Values(
        StandardsApplicableStatus.NotStarted,
        StandardsApplicableStatus.Draft,
        StandardsApplicableStatus.Submitted,
        StandardsApplicableStatus.Remediation,
        StandardsApplicableStatus.Approved,
        StandardsApplicableStatus.ApprovedPartial,
        StandardsApplicableStatus.ApprovedFirstOfType,
        StandardsApplicableStatus.Rejected)]
          StandardsApplicableStatus newStatus,
      [Values(
        Roles.Admin,
        Roles.Buyer,
        Roles.Supplier)]
          string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new StandardsApplicableValidator(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var oldClaim = Creator.GetStandardsApplicable(status: oldStatus);
      var newClaim = Creator.GetStandardsApplicable(status: newStatus);
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
