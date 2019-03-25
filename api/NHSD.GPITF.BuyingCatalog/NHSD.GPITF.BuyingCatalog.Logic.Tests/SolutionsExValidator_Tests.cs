using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Logic.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class SolutionsExValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<SolutionsExValidator>> _logger;
    private Mock<ISolutionsValidator> _solutionsValidator;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<SolutionsExValidator>>();
      _solutionsValidator = new Mock<ISolutionsValidator>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object));
    }

    [Test]
    public void ClaimedCapabilityMustBelongToSolution_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      soln.ClaimedCapability = new List<CapabilitiesImplemented>
      (
        new []
        {
          Creator.GetCapabilitiesImplemented(solnId: soln.Solution.Id),
          Creator.GetCapabilitiesImplemented(solnId: soln.Solution.Id),
          Creator.GetCapabilitiesImplemented(solnId: soln.Solution.Id)
        }
      );

      validator.ClaimedCapabilityMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedCapabilityMustBelongToSolution_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      soln.ClaimedCapability = new List<CapabilitiesImplemented>
      (
        new []
        {
          Creator.GetCapabilitiesImplemented(),
          Creator.GetCapabilitiesImplemented(),
          Creator.GetCapabilitiesImplemented()
        }
      );

      validator.ClaimedCapabilityMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedCapability must belong to solution")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedStandardMustBelongToSolution_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      soln.ClaimedStandard = new List<StandardsApplicable>
      (
        new []
        {
          Creator.GetStandardsApplicable(solnId: soln.Solution.Id),
          Creator.GetStandardsApplicable(solnId: soln.Solution.Id),
          Creator.GetStandardsApplicable(solnId: soln.Solution.Id)
        }
      );

      validator.ClaimedStandardMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedStandardMustBelongToSolution_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      soln.ClaimedStandard = new List<StandardsApplicable>
      (
        new []
        {
          Creator.GetStandardsApplicable(),
          Creator.GetStandardsApplicable(),
          Creator.GetStandardsApplicable()
        }
      );

      validator.ClaimedStandardMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedStandard must belong to solution")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedCapabilityEvidenceMustBelongToClaim_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claim = Creator.GetCapabilitiesImplemented();
      var claimEv = Creator.GetCapabilitiesImplementedEvidence(claimId: claim.Id);
      soln.ClaimedCapability = new List<CapabilitiesImplemented>(new [] { claim });
      soln.ClaimedCapabilityEvidence = new List<CapabilitiesImplementedEvidence>(new[] { claimEv });

      validator.ClaimedCapabilityEvidenceMustBelongToClaim();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedCapabilityEvidenceMustBelongToClaim_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claim = Creator.GetCapabilitiesImplemented();
      var claimEv = Creator.GetCapabilitiesImplementedEvidence();
      soln.ClaimedCapability = new List<CapabilitiesImplemented>(new [] { claim });
      soln.ClaimedCapabilityEvidence = new List<CapabilitiesImplementedEvidence>(new[] { claimEv });

      validator.ClaimedCapabilityEvidenceMustBelongToClaim();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedCapabilityEvidence must belong to claim")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedStandardEvidenceMustBelongToClaim_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claim = Creator.GetStandardsApplicable();
      var claimEv = Creator.GetStandardsApplicableEvidence(claimId: claim.Id);
      soln.ClaimedStandard = new List<StandardsApplicable>(new [] { claim });
      soln.ClaimedStandardEvidence = new List<StandardsApplicableEvidence>(new[] { claimEv });

      validator.ClaimedStandardEvidenceMustBelongToClaim();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedStandardEvidenceMustBelongToClaim_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claim = Creator.GetStandardsApplicable();
      var claimEv = Creator.GetStandardsApplicableEvidence();
      soln.ClaimedStandard = new List<StandardsApplicable>(new [] { claim });
      soln.ClaimedStandardEvidence = new List<StandardsApplicableEvidence>(new[] { claimEv });

      validator.ClaimedStandardEvidenceMustBelongToClaim();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedStandardEvidence must belong to claim")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedCapabilityReviewMustBelongToEvidence_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv = Creator.GetCapabilitiesImplementedEvidence();
      var review = Creator.GetCapabilitiesImplementedReviews(evidenceId: claimEv.Id);
      soln.ClaimedCapabilityEvidence = new List<CapabilitiesImplementedEvidence>(new[] { claimEv });
      soln.ClaimedCapabilityReview = new List<CapabilitiesImplementedReviews>(new[] { review });

      validator.ClaimedCapabilityReviewMustBelongToEvidence();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedCapabilityReviewMustBelongToEvidence_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv = Creator.GetCapabilitiesImplementedEvidence();
      var review = Creator.GetCapabilitiesImplementedReviews();
      soln.ClaimedCapabilityEvidence = new List<CapabilitiesImplementedEvidence>(new[] { claimEv });
      soln.ClaimedCapabilityReview = new List<CapabilitiesImplementedReviews>(new[] { review });

      validator.ClaimedCapabilityReviewMustBelongToEvidence();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedCapabilityReview must belong to evidence")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedStandardReviewMustBelongToEvidence_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv = Creator.GetStandardsApplicableEvidence();
      var review = Creator.GetStandardsApplicableReviews(evidenceId: claimEv.Id);
      soln.ClaimedStandardEvidence = new List<StandardsApplicableEvidence>(new[] { claimEv });
      soln.ClaimedStandardReview = new List<StandardsApplicableReviews>(new[] { review });

      validator.ClaimedStandardReviewMustBelongToEvidence();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedStandardReviewMustBelongToEvidence_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv = Creator.GetStandardsApplicableEvidence();
      var review = Creator.GetStandardsApplicableReviews();
      soln.ClaimedStandardEvidence = new List<StandardsApplicableEvidence>(new[] { claimEv });
      soln.ClaimedStandardReview = new List<StandardsApplicableReviews>(new[] { review });

      validator.ClaimedStandardReviewMustBelongToEvidence();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedStandardReview must belong to evidence")
        .And
        .HaveCount(1);
    }

    [Test]
    public void TechnicalContactMustBelongToSolution_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var techCont = Creator.GetTechnicalContact(solutionId: soln.Solution.Id);
      soln.TechnicalContact = new List<TechnicalContacts>(new[] { techCont });

      validator.TechnicalContactMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void TechnicalContactMustBelongToSolution_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var techCont = Creator.GetTechnicalContact();
      soln.TechnicalContact = new List<TechnicalContacts>(new[] { techCont });

      validator.TechnicalContactMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "TechnicalContact must belong to solution")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedCapabilityEvidencePreviousVersionMustBelongToSolution_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv1 = Creator.GetCapabilitiesImplementedEvidence();
      var claimEv2 = Creator.GetCapabilitiesImplementedEvidence(prevId: claimEv1.Id);
      soln.ClaimedCapabilityEvidence = new List<CapabilitiesImplementedEvidence>(new[] { claimEv1, claimEv2 });

      validator.ClaimedCapabilityEvidencePreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedCapabilityEvidencePreviousVersionMustBelongToSolution_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv1 = Creator.GetCapabilitiesImplementedEvidence();
      var claimEv2 = Creator.GetCapabilitiesImplementedEvidence(prevId: Guid.NewGuid().ToString());
      soln.ClaimedCapabilityEvidence = new List<CapabilitiesImplementedEvidence>(new[] { claimEv1, claimEv2 });

      validator.ClaimedCapabilityEvidencePreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedCapabilityEvidence previous version must belong to solution")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedStandardEvidencePreviousVersionMustBelongToSolution_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv1 = Creator.GetStandardsApplicableEvidence();
      var claimEv2 = Creator.GetStandardsApplicableEvidence(prevId: claimEv1.Id);
      soln.ClaimedStandardEvidence = new List<StandardsApplicableEvidence>(new[] { claimEv1, claimEv2 });

      validator.ClaimedStandardEvidencePreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedStandardEvidencePreviousVersionMustBelongToSolution_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var claimEv1 = Creator.GetStandardsApplicableEvidence();
      var claimEv2 = Creator.GetStandardsApplicableEvidence(prevId: Guid.NewGuid().ToString());
      soln.ClaimedStandardEvidence = new List<StandardsApplicableEvidence>(new[] { claimEv1, claimEv2 });

      validator.ClaimedStandardEvidencePreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedStandardEvidence previous version must belong to solution")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedCapabilityReviewPreviousVersionMustBelongToSolution_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var review1 = Creator.GetCapabilitiesImplementedReviews();
      var review2 = Creator.GetCapabilitiesImplementedReviews(prevId: review1.Id);
      soln.ClaimedCapabilityReview = new List<CapabilitiesImplementedReviews>(new[] { review1, review2 });

      validator.ClaimedCapabilityReviewPreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedCapabilityReviewPreviousVersionMustBelongToSolution_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var review1 = Creator.GetCapabilitiesImplementedReviews();
      var review2 = Creator.GetCapabilitiesImplementedReviews(prevId: Guid.NewGuid().ToString());
      soln.ClaimedCapabilityReview = new List<CapabilitiesImplementedReviews>(new[] { review1, review2 });

      validator.ClaimedCapabilityReviewPreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedCapabilityReview previous version must belong to solution")
        .And
        .HaveCount(1);
    }

    [Test]
    public void ClaimedStandardReviewPreviousVersionMustBelongToSolution_Valid_Succeeds()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var review1 = Creator.GetStandardsApplicableReviews();
      var review2 = Creator.GetStandardsApplicableReviews(prevId: review1.Id);
      soln.ClaimedStandardReview = new List<StandardsApplicableReviews>(new[] { review1, review2 });

      validator.ClaimedStandardReviewPreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void ClaimedStandardReviewPreviousVersionMustBelongToSolution_Invalid_ReturnsError()
    {
      var validator = new SolutionsExValidator(_context.Object, _logger.Object, _solutionsValidator.Object);
      var soln = Creator.GetSolutionEx();
      var review1 = Creator.GetStandardsApplicableReviews();
      var review2 = Creator.GetStandardsApplicableReviews(prevId: Guid.NewGuid().ToString());
      soln.ClaimedStandardReview = new List<StandardsApplicableReviews>(new[] { review1, review2 });

      validator.ClaimedStandardReviewPreviousVersionMustBelongToSolution();
      var valres = validator.Validate(soln);

      valres.Errors.Should()
        .Contain(x => x.ErrorMessage == "ClaimedStandardReview previous version must belong to solution")
        .And
        .HaveCount(1);
    }
  }
}
