using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class SolutionsValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<SolutionsValidator>> _logger;
    private Mock<ISolutionsDatastore> _solutionDatastore;
    private Mock<IOrganisationsDatastore> _organisationDatastore;
    private Mock<IHostingEnvironment> _env;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<SolutionsValidator>>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
      _organisationDatastore = new Mock<IOrganisationsDatastore>();
      _env = new Mock<IHostingEnvironment>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object));
    }

    [Test]
    public void MustBeValidId_Null_ReturnsError()
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution();
      soln.Id = null;

      validator.MustBeValidId();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "Invalid Id")
        .And
        .Contain(x => x.ErrorMessage == "'Id' must not be empty.")
        .And
        .HaveCount(2);
    }

    [Test]
    public void MustBeValidId_NotGuid_ReturnsError()
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(id: "some other Id");

      validator.MustBeValidId();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid Id")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeValidOrganisationId_Null_ReturnsError()
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(orgId: null);
      soln.OrganisationId = null;

      validator.MustBeValidOrganisationId();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "Invalid OrganisationId")
        .And
        .Contain(x => x.ErrorMessage == "'Organisation Id' must not be empty.")
        .And
        .HaveCount(2);
    }

    [Test]
    public void MustBeValidOrganisationId_NotGuid_ReturnsError()
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(orgId: "some other Id");

      validator.MustBeValidOrganisationId();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid OrganisationId")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeSameOrganisation_Different_ReturnsError()
    {
      var orgId = Guid.NewGuid().ToString();
      var soln = Creator.GetSolution(orgId: orgId);
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(Creator.GetSolution());

      validator.MustBeSameOrganisation();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Cannot transfer solutions between organisations")
        .And
        .HaveCount(1);
    }

    #region transitions
    [TestCase(SolutionStatus.Draft, SolutionStatus.Draft, Roles.Supplier)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.Registered, Roles.Supplier)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.Registered, Roles.Supplier)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.CapabilitiesAssessment, Roles.Supplier)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.Failed, Roles.Admin)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.StandardsCompliance, Roles.Admin)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.StandardsCompliance, Roles.Supplier)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.StandardsCompliance, Roles.Supplier)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.Failed, Roles.Admin)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.FinalApproval, Roles.Admin)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.SolutionPage, Roles.Admin)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.Approved, Roles.Admin)]
    #endregion
    public void MustBeValidStatusTransition_Valid_Succeeds(SolutionStatus oldStatus, SolutionStatus newStatus, string role)
    {
      var solnId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var oldSoln = Creator.GetSolution(id: solnId, status: oldStatus);
      var newSoln = Creator.GetSolution(id: solnId, status: newStatus);
      _solutionDatastore.Setup(x => x.ById(solnId)).Returns(oldSoln);

      validator.MustBeValidStatusTransition();
      var valres = validator.Validate(newSoln);

      valres.Errors.Should().BeEmpty();
    }

    #region transitions
    [TestCase(SolutionStatus.Draft, SolutionStatus.Failed, Roles.Buyer)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.CapabilitiesAssessment, Roles.Buyer)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.StandardsCompliance, Roles.Buyer)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.SolutionPage, Roles.Buyer)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.FinalApproval, Roles.Buyer)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.Approved, Roles.Buyer)]

    [TestCase(SolutionStatus.Draft, SolutionStatus.Failed, Roles.Admin)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.CapabilitiesAssessment, Roles.Admin)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.StandardsCompliance, Roles.Admin)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.SolutionPage, Roles.Admin)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.FinalApproval, Roles.Admin)]
    [TestCase(SolutionStatus.Draft, SolutionStatus.Approved, Roles.Admin)]

    [TestCase(SolutionStatus.Registered, SolutionStatus.Failed, Roles.Buyer)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.Draft, Roles.Buyer)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.Registered, Roles.Buyer)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.StandardsCompliance, Roles.Buyer)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.SolutionPage, Roles.Buyer)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.FinalApproval, Roles.Buyer)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.Approved, Roles.Buyer)]

    [TestCase(SolutionStatus.Registered, SolutionStatus.Failed, Roles.Admin)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.Draft, Roles.Admin)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.Registered, Roles.Admin)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.StandardsCompliance, Roles.Admin)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.SolutionPage, Roles.Admin)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.FinalApproval, Roles.Admin)]
    [TestCase(SolutionStatus.Registered, SolutionStatus.Approved, Roles.Admin)]

    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.Draft, Roles.Buyer)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.Registered, Roles.Buyer)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.CapabilitiesAssessment, Roles.Buyer)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.SolutionPage, Roles.Buyer)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.FinalApproval, Roles.Buyer)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.Approved, Roles.Buyer)]

    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.Draft, Roles.Supplier)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.Registered, Roles.Supplier)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.CapabilitiesAssessment, Roles.Supplier)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.SolutionPage, Roles.Supplier)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.FinalApproval, Roles.Supplier)]
    [TestCase(SolutionStatus.CapabilitiesAssessment, SolutionStatus.Approved, Roles.Supplier)]

    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.Draft, Roles.Buyer)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.Registered, Roles.Buyer)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.CapabilitiesAssessment, Roles.Buyer)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.StandardsCompliance, Roles.Buyer)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.SolutionPage, Roles.Buyer)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.Approved, Roles.Buyer)]

    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.Draft, Roles.Supplier)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.Registered, Roles.Supplier)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.CapabilitiesAssessment, Roles.Supplier)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.SolutionPage, Roles.Supplier)]
    [TestCase(SolutionStatus.StandardsCompliance, SolutionStatus.Approved, Roles.Supplier)]

    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Failed, Roles.Buyer)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Draft, Roles.Buyer)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Registered, Roles.Buyer)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.CapabilitiesAssessment, Roles.Buyer)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.StandardsCompliance, Roles.Buyer)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.FinalApproval, Roles.Buyer)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Approved, Roles.Buyer)]

    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Failed, Roles.Supplier)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Draft, Roles.Supplier)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Registered, Roles.Supplier)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.CapabilitiesAssessment, Roles.Supplier)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.StandardsCompliance, Roles.Supplier)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.FinalApproval, Roles.Supplier)]
    [TestCase(SolutionStatus.FinalApproval, SolutionStatus.Approved, Roles.Supplier)]

    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.Failed, Roles.Buyer)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.Draft, Roles.Buyer)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.Registered, Roles.Buyer)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.CapabilitiesAssessment, Roles.Buyer)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.StandardsCompliance, Roles.Buyer)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.SolutionPage, Roles.Buyer)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.FinalApproval, Roles.Buyer)]

    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.Failed, Roles.Supplier)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.Draft, Roles.Supplier)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.Registered, Roles.Supplier)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.CapabilitiesAssessment, Roles.Supplier)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.StandardsCompliance, Roles.Supplier)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.SolutionPage, Roles.Supplier)]
    [TestCase(SolutionStatus.SolutionPage, SolutionStatus.FinalApproval, Roles.Supplier)]
    #endregion
    public void MustBeValidStatusTransition_Invalid_ReturnsError(SolutionStatus oldStatus, SolutionStatus newStatus, string role)
    {
      var solnId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var oldSoln = Creator.GetSolution(id: solnId, status: oldStatus);
      var newSoln = Creator.GetSolution(id: solnId, status: newStatus);
      _solutionDatastore.Setup(x => x.ById(solnId)).Returns(oldSoln);

      validator.MustBeValidStatusTransition();
      var valres = validator.Validate(newSoln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid Status transition")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeValidStatusTransition_FinalState_ReturnsError(
    #region params
      [Values(
        SolutionStatus.Approved,
        SolutionStatus.Failed)]
          SolutionStatus oldStatus,
      [Values(
        SolutionStatus.Failed,
        SolutionStatus.Draft,
        SolutionStatus.Registered,
        SolutionStatus.CapabilitiesAssessment,
        SolutionStatus.StandardsCompliance,
        SolutionStatus.SolutionPage,
        SolutionStatus.FinalApproval,
        SolutionStatus.Approved)]
          SolutionStatus newStatus,
      [Values(
        Roles.Admin,
        Roles.Buyer,
        Roles.Supplier)]
    #endregion
          string role)
    {
      var solnId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var oldSoln = Creator.GetSolution(id: solnId, status: oldStatus);
      var newSoln = Creator.GetSolution(id: solnId, status: newStatus);
      _solutionDatastore.Setup(x => x.ById(solnId)).Returns(oldSoln);

      validator.MustBeValidStatusTransition();
      var valres = validator.Validate(newSoln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid Status transition")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeFromSameOrganisationOrAdmin_Same_Succeeds()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId, role: Roles.Supplier));
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(orgId: orgId);

      validator.MustBeFromSameOrganisationOrAdmin();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeFromSameOrganisationOrAdmin_Different_ReturnsError(
      [Values(
        Roles.Buyer,
        Roles.Supplier)]
          string role)
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: role));
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution();

      validator.MustBeFromSameOrganisationOrAdmin();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Must be from same organisation")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeFromSameOrganisationOrAdmin_Admin_Succeeds()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(role: Roles.Admin));
      var orgId = Guid.NewGuid().ToString();
      var soln = Creator.GetSolution(orgId: orgId);
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(Creator.GetSolution());

      validator.MustBeFromSameOrganisationOrAdmin();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeCurrentVersion_Current_Succeeds()
    {
      var solnId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var oldSoln = Creator.GetSolution(id: solnId);
      var newSoln = Creator.GetSolution(id: solnId);
      _solutionDatastore.Setup(x => x.ByOrganisation(newSoln.OrganisationId)).Returns(new[] { oldSoln });

      validator.MustBeCurrentVersion();
      var valres = validator.Validate(newSoln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeCurrentVersion_PreviousVersion_ReturnsError()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByOrganisation(soln.OrganisationId)).Returns(new[] { Creator.GetSolution(previousId: soln.Id) });

      validator.MustBeCurrentVersion();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Can only change current version")
        .And
        .HaveCount(1);
    }

    [Test]
    public void PreviousVersionMustBeFromSameOrganisation_NoPrevious_Succeeds()
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution();

      validator.PreviousVersionMustBeFromSameOrganisation();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void PreviousVersionMustBeFromSameOrganisation_Different_ReturnsError()
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(previousId: Guid.NewGuid().ToString());
      _solutionDatastore.Setup(x => x.ById(soln.PreviousId)).Returns(Creator.GetSolution());

      validator.PreviousVersionMustBeFromSameOrganisation();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Previous version must be from same organisation")
        .And
        .HaveCount(1);
    }

    [TestCase(SolutionStatus.Draft)]
    public void MustBePending_Draft_Succeeds(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);

      validator.MustBePending();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    #region statuses
    [TestCase(SolutionStatus.Failed)]
    [TestCase(SolutionStatus.Registered)]
    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    [TestCase(SolutionStatus.StandardsCompliance)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    #endregion
    public void MustBePending_NonDraft_ReturnsError(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);

      validator.MustBePending();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Status must be Draft")
        .And
        .HaveCount(1);
    }

    public static IEnumerable<string> NonDevelopmentEnvironments()
    {
      yield return EnvironmentName.Production;
      yield return EnvironmentName.Staging;
      yield return string.Empty;
      yield return null;
    }

    public static IEnumerable<string> DevelopmentEnvironments()
    {
      yield return EnvironmentName.Development;
    }

    [Test]
    public void MustBeDevelopment_NonDevelopment_ReturnsError(
      [ValueSource(nameof(NonDevelopmentEnvironments))]string environment)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution();
      _env.Setup(x => x.EnvironmentName).Returns(environment);

      validator.MustBeDevelopment();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Only available in Development environment")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeDevelopment_Development_Succeeds(
      [ValueSource(nameof(DevelopmentEnvironments))]string environment)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution();
      _env.Setup(x => x.EnvironmentName).Returns(environment);

      validator.MustBeDevelopment();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    #region statuses
    [TestCase(SolutionStatus.Failed)]
    [TestCase(SolutionStatus.Registered)]
    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    [TestCase(SolutionStatus.StandardsCompliance)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    #endregion
    public void MustBePendingToChangeName_NonDraft_ReturnsError(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);
      soln.Name = "New name";
      var solnOld = Creator.GetSolution(status: status, id:soln.Id);
      solnOld.Name = "Original name";
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(solnOld);

      validator.MustBePendingToChangeName();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Can only change name in Draft")
        .And
        .HaveCount(1);
    }

    [TestCase(SolutionStatus.Draft)]
    public void MustBePendingToChangeName_Draft_Succeeds(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);
      soln.Name = "New name";
      var solnOld = Creator.GetSolution(status: status, id:soln.Id);
      solnOld.Name = "Original name";
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(solnOld);

      validator.MustBePendingToChangeName();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    #region statuses
    [TestCase(SolutionStatus.Failed)]
    [TestCase(SolutionStatus.Draft)]
    [TestCase(SolutionStatus.Registered)]
    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    [TestCase(SolutionStatus.StandardsCompliance)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    #endregion
    public void MustBePendingToChangeName_SameName_Succeeds(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);
      soln.Name = "Original name";
      var solnOld = Creator.GetSolution(status: status, id:soln.Id);
      solnOld.Name = "Original name";
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(solnOld);

      validator.MustBePendingToChangeName();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }



    #region statuses
    [TestCase(SolutionStatus.Failed)]
    [TestCase(SolutionStatus.Registered)]
    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    [TestCase(SolutionStatus.StandardsCompliance)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    #endregion
    public void MustBePendingToChangeVersion_NonDraft_ReturnsError(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);
      soln.Version = "New version";
      var solnOld = Creator.GetSolution(status: status, id:soln.Id);
      solnOld.Version = "Original version";
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(solnOld);

      validator.MustBePendingToChangeVersion();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Can only change version in Draft")
        .And
        .HaveCount(1);
    }

    [TestCase(SolutionStatus.Draft)]
    public void MustBePendingToChangeVersion_Draft_Succeeds(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);
      soln.Version = "New version";
      var solnOld = Creator.GetSolution(status: status, id:soln.Id);
      solnOld.Version = "Original version";
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(solnOld);

      validator.MustBePendingToChangeVersion();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    #region statuses
    [TestCase(SolutionStatus.Failed)]
    [TestCase(SolutionStatus.Draft)]
    [TestCase(SolutionStatus.Registered)]
    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    [TestCase(SolutionStatus.StandardsCompliance)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    #endregion
    public void MustBePendingToChangeVersion_SameVersion_Succeeds(SolutionStatus status)
    {
      var validator = new SolutionsValidator(_context.Object, _logger.Object, _solutionDatastore.Object, _organisationDatastore.Object, _env.Object);
      var soln = Creator.GetSolution(status: status);
      soln.Version = "Original version";
      var solnOld = Creator.GetSolution(status: status, id:soln.Id);
      solnOld.Version = "Original version";
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(solnOld);

      validator.MustBePendingToChangeVersion();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

  }
}
