using System;
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
  public sealed class CapabilitiesImplementedReviewsValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<CapabilitiesImplementedReviewsValidator>> _logger;
    private Mock<ICapabilitiesImplementedReviewsDatastore> _reviewsDatastore;
    private Mock<ICapabilitiesImplementedEvidenceDatastore> _evidenceDatastore;
    private Mock<ICapabilitiesImplementedDatastore> _claimDatastore;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<CapabilitiesImplementedReviewsValidator>>();
      _reviewsDatastore = new Mock<ICapabilitiesImplementedReviewsDatastore>();
      _reviewsDatastore.As<IReviewsDatastore<ReviewsBase>>();
      _evidenceDatastore = new Mock<ICapabilitiesImplementedEvidenceDatastore>();
      _evidenceDatastore.As<IEvidenceDatastore<EvidenceBase>>();
      _claimDatastore = new Mock<ICapabilitiesImplementedDatastore>();
      _claimDatastore.As<IClaimsDatastore<ClaimsBase>>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new CapabilitiesImplementedReviewsValidator(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object));
    }

    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    public void SolutionMustBeInReview_Review_Succeeds(SolutionStatus status)
    {
      var validator = new CapabilitiesImplementedReviewsValidator(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(status: status);
      var review = GetCapabilitiesImplementedReview();
      var claim = Creator.GetCapabilitiesImplemented(solnId: soln.Id);
      var evidence = Creator.GetCapabilitiesImplementedEvidence(claimId: claim.Id);
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
    [TestCase(SolutionStatus.StandardsCompliance)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    public void SolutionMustBeInReview_NonReview_ReturnsError(SolutionStatus status)
    {
      var validator = new CapabilitiesImplementedReviewsValidator(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(status: status);
      var review = GetCapabilitiesImplementedReview();
      var claim = Creator.GetCapabilitiesImplemented(solnId: soln.Id);
      var evidence = Creator.GetCapabilitiesImplementedEvidence(claimId: claim.Id);
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

    private static CapabilitiesImplementedReviews GetCapabilitiesImplementedReview(
      string id = null,
      string prevId = null,
      string evidenceId = null)
    {
      return new CapabilitiesImplementedReviews
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        EvidenceId = evidenceId ?? Guid.NewGuid().ToString()
      };
    }
  }
}
