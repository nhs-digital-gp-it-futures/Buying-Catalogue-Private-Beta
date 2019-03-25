using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class ClaimsValidatorBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<DummyClaimsValidatorBase>> _logger;
    private Mock<IClaimsDatastore<ClaimsBase>> _claimDatastore;
    private Mock<IContactsDatastore> _contactsDatastore;
    private Mock<ISolutionsDatastore> _solutionsDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<DummyClaimsValidatorBase>>();
      _claimDatastore = new Mock<IClaimsDatastore<ClaimsBase>>();
      _contactsDatastore = new Mock<IContactsDatastore>();
      _solutionsDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object));
    }

    [Test]
    public void MustBeValidId_Valid_Succeeds()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();

      validator.MustBeValidId();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidId_Null_ReturnsError()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      claim.Id = null;

      validator.MustBeValidId();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid Id")
        .And
        .ContainSingle(x => x.ErrorMessage == "'Id' must not be empty.")
        .And
        .HaveCount(2);
    }

    [Test]
    public void MustBeValidId_NotGuid_ReturnsError()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase(id: "some other Id");

      validator.MustBeValidId();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid Id")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeValidSolutionId_Valid_Succeeds()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();

      validator.MustBeValidSolutionId();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidSolutionId_Null_ReturnsError()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      claim.SolutionId = null;

      validator.MustBeValidSolutionId();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid SolutionId")
        .And
        .ContainSingle(x => x.ErrorMessage == "'Solution Id' must not be empty.")
        .And
        .HaveCount(2);
    }

    [Test]
    public void MustBeValidSolutionId_NotGuid_ReturnsError()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase(solnId: "some other Id");

      validator.MustBeValidSolutionId();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid SolutionId")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeSameSolution_Same_Succeeds()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      _claimDatastore.Setup(x => x.ById(claim.Id)).Returns(claim);

      validator.MustBeSameSolution();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeSameSolution_Different_ReturnsError()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      _claimDatastore.Setup(x => x.ById(claim.Id)).Returns(Creator.GetClaimsBase());

      validator.MustBeSameSolution();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Cannot transfer claim between solutions")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeSameOrganisation_Same_Succeeds()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId));
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      _claimDatastore.Setup(x => x.ById(claim.Id)).Returns(claim);
      _solutionsDatastore.Setup(x => x.ById(claim.SolutionId)).Returns(Creator.GetSolution(orgId: orgId));

      validator.MustBeSameOrganisation();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeSameOrganisation_Same_NewClaim_Succeeds()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId));
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      _solutionsDatastore.Setup(x => x.ById(claim.SolutionId)).Returns(Creator.GetSolution(orgId: orgId));

      validator.MustBeSameOrganisation();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeSameOrganisation_Different_ReturnsError()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId));
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      _claimDatastore.Setup(x => x.ById(claim.Id)).Returns(claim);
      _solutionsDatastore.Setup(x => x.ById(claim.SolutionId)).Returns(Creator.GetSolution());

      validator.MustBeSameOrganisation();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Cannot create/change claim for other organisation")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ContactMustBeSameOrganisation_Same_Succeeds()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId));
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var contact = Creator.GetContact(orgId: orgId);
      var claim = Creator.GetClaimsBase(ownerId: contact.Id);
      _contactsDatastore.Setup(x => x.ById(contact.Id)).Returns(contact);

      validator.OwnerMustBeSameOrganisation();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ContactMustBeSameOrganisation_Different_ReturnsError()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId));
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var contact = Creator.GetContact(orgId: orgId);
      var claim = Creator.GetClaimsBase();
      _contactsDatastore.Setup(x => x.ById(contact.Id)).Returns(contact);

      validator.OwnerMustBeSameOrganisation();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Contact must be from organisation")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeValidOwnerId_Valid_Succeeds()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();

      validator.MustBeValidOwnerId();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidOwnerId_Null_Succeeds()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase();
      claim.OwnerId = null;

      validator.MustBeValidOwnerId();
      var valres = validator.Validate(claim);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidOwnerId_NotGuid_ReturnsError()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase(ownerId: "some other Id");

      validator.MustBeValidOwnerId();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid OwnerId")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeValidOwnerId_Empty_ReturnsError()
    {
      var validator = new DummyClaimsValidatorBase(_context.Object, _logger.Object, _claimDatastore.Object, _contactsDatastore.Object, _solutionsDatastore.Object);
      var claim = Creator.GetClaimsBase(ownerId: string.Empty);

      validator.MustBeValidOwnerId();
      var valres = validator.Validate(claim);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid OwnerId")
        .And
        .HaveCount(1);
    }
  }
}
