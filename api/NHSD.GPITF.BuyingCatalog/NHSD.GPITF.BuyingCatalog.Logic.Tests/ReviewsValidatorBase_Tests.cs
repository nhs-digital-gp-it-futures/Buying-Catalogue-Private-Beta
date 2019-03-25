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
  public sealed class ReviewsValidatorBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<DummyReviewsValidatorBase>> _logger;
    private Mock<IReviewsDatastore<ReviewsBase>> _reviewsDatastore;
    private Mock<IEvidenceDatastore<EvidenceBase>> _evidenceDatastore;
    private Mock<IClaimsDatastore<ClaimsBase>> _claimDatastore;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<DummyReviewsValidatorBase>>();
      _reviewsDatastore = new Mock<IReviewsDatastore<ReviewsBase>>();
      _evidenceDatastore = new Mock<IEvidenceDatastore<EvidenceBase>>();
      _claimDatastore = new Mock<IClaimsDatastore<ClaimsBase>>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object));
    }

    [Test]
    public void MustBeValidEvidenceId_Valid_Succeeds()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase();

      validator.MustBeValidEvidenceId();
      var valres = validator.Validate(review);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidEvidenceId_Null_ReturnsError()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase();
      review.EvidenceId = null;

      validator.MustBeValidEvidenceId();
      var valres = validator.Validate(review);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid EvidenceId")
        .And
        .ContainSingle(x => x.ErrorMessage == "'Evidence Id' must not be empty.")
        .And
        .HaveCount(2);
    }

    [Test]
    public void MustBeValidEvidenceId_NotGuid_ReturnsError()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase(evidenceId: "some other Id");

      validator.MustBeValidEvidenceId();
      var valres = validator.Validate(review);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid EvidenceId")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeValidPreviousId_Valid_Succeeds()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase();

      validator.MustBeValidPreviousId();
      var valres = validator.Validate(review);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidPreviousId_Null_Succeeds()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase(prevId: null);

      validator.MustBeValidPreviousId();
      var valres = validator.Validate(review);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidPreviousId_NotGuid_ReturnsError()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase(prevId: "not a GUID");

      validator.MustBeValidPreviousId();
      var valres = validator.Validate(review);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid PreviousId")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeFromSameOrganisation_Same_Succeeds()
    {
      var orgId = Guid.NewGuid().ToString();
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId));
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var evidence = Creator.GetEvidenceBase(claimId: claim.Id);
      _evidenceDatastore.Setup(x => x.ById(review.EvidenceId)).Returns(evidence);
      _claimDatastore.Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.MustBeFromSameOrganisation();
      var valres = validator.Validate(review);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeFromSameOrganisation_Other_ReturnsError()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase();
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var evidence = Creator.GetEvidenceBase();
      _evidenceDatastore.Setup(x => x.ById(review.EvidenceId)).Returns(evidence);
      _claimDatastore.Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.MustBeFromSameOrganisation();
      var valres = validator.Validate(review);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Must be from same organisation")
        .And
        .HaveCount(1);
    }

    [Test]
    public void PreviousMustBeForSameEvidence_Same_Succeeds()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var prevReview = Creator.GetReviewsBase();
      var review = Creator.GetReviewsBase(prevId: prevReview.Id, evidenceId: prevReview.EvidenceId);
      var prevEvidence = Creator.GetEvidenceBase();
      _reviewsDatastore.Setup(x => x.ById(prevReview.Id)).Returns(prevReview);
      _evidenceDatastore.Setup(x => x.ById(prevReview.EvidenceId)).Returns(prevEvidence);

      validator.PreviousMustBeForSameEvidence();
      var valres = validator.Validate(review);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void PreviousMustBeForSameEvidence_Other_ReturnsError()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var prevReview = Creator.GetReviewsBase();
      var review = Creator.GetReviewsBase(prevId: prevReview.Id);
      var prevEvidence = Creator.GetEvidenceBase();
      _reviewsDatastore.Setup(x => x.ById(prevReview.Id)).Returns(prevReview);
      _evidenceDatastore.Setup(x => x.ById(prevReview.EvidenceId)).Returns(prevEvidence);

      validator.PreviousMustBeForSameEvidence();
      var valres = validator.Validate(review);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Previous review must be for same evidence")
        .And
        .HaveCount(1);
    }

    [Test]
    public void PreviousMustNotBeInUse_NotInUse_Succeeds()
    {
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var review = Creator.GetReviewsBase(prevId: Guid.NewGuid().ToString());
      _reviewsDatastore.Setup(x => x.ByEvidence(review.EvidenceId)).Returns(new[] { new[] { Creator.GetReviewsBase() } });

      validator.PreviousMustNotBeInUse();
      var valres = validator.Validate(review);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void PreviousMustNotBeInUse_InUse_ReturnsError()
    {
      var evidenceId = Guid.NewGuid().ToString();

      // first chain: rev1 <-- rev2
      var rev1 = Creator.GetReviewsBase(evidenceId: evidenceId);
      var rev2 = Creator.GetReviewsBase(evidenceId: evidenceId, prevId: rev1.Id);

      // second chain: revA <-- revB
      var revA = Creator.GetReviewsBase(evidenceId: evidenceId);
      var revB = Creator.GetReviewsBase(evidenceId: evidenceId, prevId: revA.Id);

      // review datastore returns both chains
      _reviewsDatastore.Setup(x => x.ByEvidence(evidenceId))
        .Returns(new[] 
        {
          new[] { rev1, rev2 },
          new[] { revA, revB }
        });

      // create new review linked (previous) to rev1 ie 'fan out'
      var review = Creator.GetReviewsBase(evidenceId: evidenceId, prevId: rev1.Id);
      var validator = new DummyReviewsValidatorBase(_reviewsDatastore.Object, _evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);


      validator.PreviousMustNotBeInUse();
      var valres = validator.Validate(review);


      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Previous review already in use")
        .And
        .HaveCount(1);
    }
  }
}
