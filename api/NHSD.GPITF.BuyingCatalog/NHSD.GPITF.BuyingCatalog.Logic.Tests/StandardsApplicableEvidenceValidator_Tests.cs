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
  public sealed class StandardsApplicableEvidenceValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<StandardsApplicableEvidenceValidator>> _logger;
    private Mock<IStandardsApplicableEvidenceDatastore> _evidenceDatastore;
    private Mock<IStandardsApplicableDatastore> _claimDatastore;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<StandardsApplicableEvidenceValidator>>();
      _evidenceDatastore = new Mock<IStandardsApplicableEvidenceDatastore>();
      _claimDatastore = new Mock<IStandardsApplicableDatastore>();
      _claimDatastore.As<IClaimsDatastore<ClaimsBase>>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new StandardsApplicableEvidenceValidator(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object));
    }

    [TestCase(SolutionStatus.StandardsCompliance)]
    public void SolutionMustBeInReview_Review_Succeeds(SolutionStatus status)
    {
      var validator = new StandardsApplicableEvidenceValidator(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(status: status);
      var claim = Creator.GetStandardsApplicable(solnId: soln.Id);
      var evidence = Creator.GetStandardsApplicableEvidence(claimId: claim.Id);
      _claimDatastore.As<IClaimsDatastore<ClaimsBase>>().Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.SolutionMustBeInReview();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [TestCase(SolutionStatus.Failed)]
    [TestCase(SolutionStatus.Draft)]
    [TestCase(SolutionStatus.Registered)]
    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    public void SolutionMustBeInReview_NonReview_ReturnsError(SolutionStatus status)
    {
      var validator = new StandardsApplicableEvidenceValidator(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(status: status);
      var claim = Creator.GetStandardsApplicable(solnId: soln.Id);
      var evidence = Creator.GetStandardsApplicableEvidence(claimId: claim.Id);
      _claimDatastore.As<IClaimsDatastore<ClaimsBase>>().Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.SolutionMustBeInReview();
      var valres = validator.Validate(evidence);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Can only add evidence if solution is in review")
        .And
        .HaveCount(1);
    }
  }
}
