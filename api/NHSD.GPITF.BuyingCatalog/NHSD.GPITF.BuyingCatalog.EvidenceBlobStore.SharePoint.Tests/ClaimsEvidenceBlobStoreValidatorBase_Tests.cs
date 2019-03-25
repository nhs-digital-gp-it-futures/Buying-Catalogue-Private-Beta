using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic.Tests;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint.Tests
{
  [TestFixture]
  public sealed class ClaimsEvidenceBlobStoreValidatorBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<DummyClaimsEvidenceBlobStoreValidatorBase>> _logger;
    private Mock<ISolutionsDatastore> _solutionsDatastore;
    private Mock<IClaimsDatastore<ClaimsBase>> _claimsDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<DummyClaimsEvidenceBlobStoreValidatorBase>>();
      _solutionsDatastore = new Mock<ISolutionsDatastore>();
      _claimsDatastore = new Mock<IClaimsDatastore<ClaimsBase>>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object));
    }

    [Test]
    public void MustBeValidClaim_Valid_Succeeds()
    {
      var claimId = Guid.NewGuid().ToString();
      _claimsDatastore.Setup(x => x.ById(claimId)).Returns(Creator.GetClaimsBase());
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeValidClaim();
      var valres = validator.Validate(claimId);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidClaim_Invalid_ReturnsError()
    {
      var claimId = Guid.NewGuid().ToString();
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeValidClaim();
      var valres = validator.Validate(claimId);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Could not find claim")
        .And
        .HaveCount(1);
    }

    [TestCase(Roles.Supplier)]
    [TestCase(Roles.Admin)]
    public void MustBeSameOrganisation_Same_NonBuyer_Succeeds(string role)
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(orgId: orgId, role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var claimId = Guid.NewGuid().ToString();
      var claim = Creator.GetClaimsBase();
      _claimsDatastore.Setup(x => x.ById(claimId)).Returns(claim);
      var soln = Creator.GetSolution(orgId: orgId);
      _solutionsDatastore.Setup(x => x.ById(claim.SolutionId)).Returns(soln);
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeSameOrganisation();
      var valres = validator.Validate(claimId);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(Roles.Supplier)]
    public void MustBeSameOrganisation_Different_Supplier_ReturnsError(string role)
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var claimId = Guid.NewGuid().ToString();
      var claim = Creator.GetClaimsBase();
      _claimsDatastore.Setup(x => x.ById(claimId)).Returns(claim);
      var soln = Creator.GetSolution(orgId: orgId);
      _solutionsDatastore.Setup(x => x.ById(claim.SolutionId)).Returns(soln);
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeSameOrganisation();
      var valres = validator.Validate(claimId);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Cannot add/see evidence for other organisation")
        .And
        .HaveCount(1);
    }

    [TestCase(Roles.Buyer)]
    public void MustBeSameOrganisation_Buyer_Ignored(string role)
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var claimId = Guid.NewGuid().ToString();
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeSameOrganisation();
      var valres = validator.Validate(claimId);

      valres.Errors.Should().BeEmpty();
      _claimsDatastore.Verify(x => x.ById(It.IsAny<string>()), Times.Never);
      _solutionsDatastore.Verify(x => x.ById(It.IsAny<string>()), Times.Never);
    }

    [TestCase(Roles.Supplier)]
    [TestCase(Roles.Admin)]
    public void MustBeSameOrganisationById_Same_NonBuyer_Succeeds(string role)
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(orgId: orgId, role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var soln = Creator.GetSolution(orgId: orgId);
      _solutionsDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeSameOrganisationById();
      var valres = validator.Validate(soln.Id);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(Roles.Supplier)]
    public void MustBeSameOrganisationById_Different_Supplier_ReturnsError(string role)
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var soln = Creator.GetSolution(orgId: orgId);
      _solutionsDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeSameOrganisationById();
      var valres = validator.Validate(soln.Id);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Cannot add/see evidence for other organisation")
        .And
        .HaveCount(1);
    }

    [TestCase(Roles.Buyer)]
    public void MustBeSameOrganisationById_Buyer_Ignored(string role)
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var claimId = Guid.NewGuid().ToString();
      var validator = new DummyClaimsEvidenceBlobStoreValidatorBase(_context.Object, _logger.Object, _solutionsDatastore.Object, _claimsDatastore.Object);

      validator.MustBeSameOrganisationById();
      var valres = validator.Validate(Guid.NewGuid().ToString());

      valres.Errors.Should().BeEmpty();
      _claimsDatastore.Verify(x => x.ById(It.IsAny<string>()), Times.Never);
      _solutionsDatastore.Verify(x => x.ById(It.IsAny<string>()), Times.Never);
    }
  }
}
