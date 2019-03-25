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
  public sealed class StandardsApplicableReviewsValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<StandardsApplicableReviewsValidator>> _logger;
    private Mock<IStandardsApplicableReviewsDatastore> _reviewsDatastore;
    private Mock<IStandardsApplicableEvidenceDatastore> _evidenceDatastore;
    private Mock<IStandardsApplicableDatastore> _claimDatastore;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<StandardsApplicableReviewsValidator>>();
      _reviewsDatastore = new Mock<IStandardsApplicableReviewsDatastore>();
      _reviewsDatastore.As<IReviewsDatastore<ReviewsBase>>();
      _evidenceDatastore = new Mock<IStandardsApplicableEvidenceDatastore>();
      _evidenceDatastore.As<IEvidenceDatastore<EvidenceBase>>();
      _claimDatastore = new Mock<IStandardsApplicableDatastore>();
      _claimDatastore.As<IClaimsDatastore<ClaimsBase>>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new StandardsApplicableReviewsValidator(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object));
    }

    [TestCase(SolutionStatus.StandardsCompliance)]
    public void SolutionMustBeInReview_Review_Succeeds(SolutionStatus status)
    {
      var validator = new StandardsApplicableReviewsValidator(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(status: status);
      var review = GetStandardsApplicableReview();
      var claim = Creator.GetStandardsApplicable(solnId: soln.Id);
      var evidence = Creator.GetStandardsApplicableEvidence(claimId: claim.Id);
      _evidenceDatastore.As<IEvidenceDatastore<EvidenceBase>>().Setup(x => x.ById(review.EvidenceId)).Returns(evidence);
      _claimDatastore.As<IClaimsDatastore<ClaimsBase>>().Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.SolutionMustBeInReview();
      var valres = validator.Validate(review);

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
      var validator = new StandardsApplicableReviewsValidator(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(status: status);
      var review = GetStandardsApplicableReview();
      var claim = Creator.GetStandardsApplicable(solnId: soln.Id);
      var evidence = Creator.GetStandardsApplicableEvidence(claimId: claim.Id);
      _evidenceDatastore.As<IEvidenceDatastore<EvidenceBase>>().Setup(x => x.ById(review.EvidenceId)).Returns(evidence);
      _claimDatastore.As<IClaimsDatastore<ClaimsBase>>().Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.SolutionMustBeInReview();
      var valres = validator.Validate(review);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Can only add evidence if solution is in review")
        .And
        .HaveCount(1);
    }

    private static StandardsApplicableReviews GetStandardsApplicableReview(
      string id = null,
      string prevId = null,
      string evidenceId = null)
    {
      return new StandardsApplicableReviews
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        EvidenceId = evidenceId ?? Guid.NewGuid().ToString()
      };
    }
  }
}
