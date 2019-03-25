using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class ReviewsValidatorBase<T> : ValidatorBase<T> where T : ReviewsBase
  {
    private readonly IReviewsDatastore<ReviewsBase> _reviewsDatastore;
    private readonly IEvidenceDatastore<EvidenceBase> _evidenceDatastore;
    private readonly IClaimsDatastore<ClaimsBase> _claimDatastore;
    private readonly ISolutionsDatastore _solutionDatastore;

    public ReviewsValidatorBase(
      IReviewsDatastore<ReviewsBase> reviewsDatastore,
      IEvidenceDatastore<EvidenceBase> evidenceDatastore,
      IClaimsDatastore<ClaimsBase> claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context,
      ILogger<ReviewsValidatorBase<T>> logger) :
      base(context, logger)
    {
      _reviewsDatastore = reviewsDatastore;
      _evidenceDatastore = evidenceDatastore;
      _claimDatastore = claimDatastore;
      _solutionDatastore = solutionDatastore;

      RuleSet(nameof(IReviewsLogic<T>.Create), () =>
      {
        MustBeValidEvidenceId();
        MustBeSupplier();
        SolutionMustBeInReview();
        MustBeFromSameOrganisation();
        MustBeValidPreviousId();
        PreviousMustBeForSameEvidence();
        PreviousMustNotBeInUse();
      });
    }

    protected abstract SolutionStatus SolutionReviewStatus { get; }

    public void MustBeValidEvidenceId()
    {
      RuleFor(x => x.EvidenceId)
        .NotNull()
        .Must(id => Guid.TryParse(id, out _))
        .WithMessage("Invalid EvidenceId");
    }

    public void MustBeValidPreviousId()
    {
      RuleFor(x => x.PreviousId)
        .Must(id => Guid.TryParse(id, out _))
        .When(x => !string.IsNullOrEmpty(x.PreviousId))
        .WithMessage("Invalid PreviousId");
    }

    public void MustBeFromSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var evidence = _evidenceDatastore.ById(x.EvidenceId);
          var claim = _claimDatastore.ById(evidence.ClaimId);
          var soln = _solutionDatastore.ById(claim.SolutionId);
          var orgId = _context.OrganisationId();
          return soln.OrganisationId == orgId;
        })
        .WithMessage("Must be from same organisation");
    }

    public void PreviousMustBeForSameEvidence()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var prevReview = _reviewsDatastore.ById(x.PreviousId);
          var prevEvidence = _evidenceDatastore.ById(prevReview.EvidenceId);
          return x.EvidenceId == prevReview.EvidenceId;
        })
        .When(x => !string.IsNullOrEmpty(x.PreviousId))
        .WithMessage("Previous review must be for same evidence");
    }

    public void SolutionMustBeInReview()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var evidence = _evidenceDatastore.ById(x.EvidenceId);
          var claim = _claimDatastore.ById(evidence.ClaimId);
          var soln = _solutionDatastore.ById(claim.SolutionId);
          return soln.Status == SolutionReviewStatus;
        })
        .WithMessage("Can only add evidence if solution is in review");
    }

    public void PreviousMustNotBeInUse()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var chains = _reviewsDatastore.ByEvidence(x.EvidenceId);
          var allPrevIds = chains.SelectMany(chain => chain.Select(review => review.PreviousId));
          return !allPrevIds.Contains(x.PreviousId);
        })
        .When(x => !string.IsNullOrEmpty(x.PreviousId))
        .WithMessage("Previous review already in use");
    }
  }
}
