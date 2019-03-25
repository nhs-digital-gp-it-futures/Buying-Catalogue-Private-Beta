using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic.Porcelain
{
  public sealed class SolutionsExValidator : ValidatorBase<SolutionEx>, ISolutionsExValidator
  {
    private readonly ISolutionsValidator _solutionsValidator;

    public SolutionsExValidator(
      IHttpContextAccessor context,
      ILogger<SolutionsExValidator> logger,
      ISolutionsValidator solutionsValidator) :
      base(context, logger)
    {
      _solutionsValidator = solutionsValidator;

      RuleSet(nameof(ISolutionsExLogic.Update), () =>
      {
        // use Solution validator
        MustBeValidSolution();

        // internal consistency checks
        ClaimedCapabilityMustBelongToSolution();
        ClaimedCapabilityEvidenceMustBelongToClaim();
        ClaimedCapabilityReviewMustBelongToEvidence();

        ClaimedStandardMustBelongToSolution();
        ClaimedStandardEvidenceMustBelongToClaim();
        ClaimedStandardReviewMustBelongToEvidence();

        TechnicalContactMustBelongToSolution();

        // all previous versions in solution
        ClaimedCapabilityEvidencePreviousVersionMustBelongToSolution();
        ClaimedStandardEvidencePreviousVersionMustBelongToSolution();

        ClaimedCapabilityReviewPreviousVersionMustBelongToSolution();
        ClaimedStandardReviewPreviousVersionMustBelongToSolution();
      });
    }

    public void MustBeValidSolution()
    {
      RuleFor(x => x.Solution)
        .Must(soln =>
        {
          _solutionsValidator.ValidateAndThrowEx(soln, ruleSet: nameof(ISolutionsLogic.Update));
          return true;
        });
    }

    public void ClaimedCapabilityMustBelongToSolution()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          return soln.ClaimedCapability.All(cc => cc.SolutionId == soln.Solution.Id);
        })
        .WithMessage("ClaimedCapability must belong to solution");
    }

    public void ClaimedStandardMustBelongToSolution()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          return soln.ClaimedStandard.All(cs => cs.SolutionId == soln.Solution.Id);
        })
        .WithMessage("ClaimedStandard must belong to solution");
    }

    public void ClaimedCapabilityEvidenceMustBelongToClaim()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var claimIds = soln.ClaimedCapability.Select(cc => cc.Id);
          return soln.ClaimedCapabilityEvidence.All(cce => claimIds.Contains(cce.ClaimId));
        })
        .WithMessage("ClaimedCapabilityEvidence must belong to claim");
    }

    public void ClaimedStandardEvidenceMustBelongToClaim()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var claimIds = soln.ClaimedStandard.Select(cs => cs.Id);
          return soln.ClaimedStandardEvidence.All(cse => claimIds.Contains(cse.ClaimId));
        })
        .WithMessage("ClaimedStandardEvidence must belong to claim");
    }

    public void ClaimedCapabilityReviewMustBelongToEvidence()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var evidenceIds = soln.ClaimedCapabilityEvidence.Select(cce => cce.Id);
          return soln.ClaimedCapabilityReview.All(ccr => evidenceIds.Contains(ccr.EvidenceId));
        })
        .WithMessage("ClaimedCapabilityReview must belong to evidence");
    }

    public void ClaimedStandardReviewMustBelongToEvidence()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var evidenceIds = soln.ClaimedStandardEvidence.Select(cse => cse.Id);
          return soln.ClaimedStandardReview.All(csr => evidenceIds.Contains(csr.EvidenceId));
        })
        .WithMessage("ClaimedStandardReview must belong to evidence");
    }

    public void TechnicalContactMustBelongToSolution()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          return soln.TechnicalContact.All(tc => tc.SolutionId == soln.Solution.Id);
        })
        .WithMessage("TechnicalContact must belong to solution");
    }

    public void ClaimedCapabilityEvidencePreviousVersionMustBelongToSolution()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var evidenceIds = soln.ClaimedCapabilityEvidence.Select(cce => cce.Id);
          var evidencePrevIds = soln.ClaimedCapabilityEvidence.Select(cce => cce.PreviousId).Where(id => id != null);
          return evidencePrevIds.All(prevId => evidenceIds.Contains(prevId));
        })
        .WithMessage("ClaimedCapabilityEvidence previous version must belong to solution");
    }

    public void ClaimedStandardEvidencePreviousVersionMustBelongToSolution()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var evidenceIds = soln.ClaimedStandardEvidence.Select(cce => cce.Id);
          var evidencePrevIds = soln.ClaimedStandardEvidence.Select(cce => cce.PreviousId).Where(id => id != null);
          return evidencePrevIds.All(prevId => evidenceIds.Contains(prevId));
        })
        .WithMessage("ClaimedStandardEvidence previous version must belong to solution");
    }

    public void ClaimedCapabilityReviewPreviousVersionMustBelongToSolution()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var evidenceIds = soln.ClaimedCapabilityReview.Select(cce => cce.Id);
          var evidencePrevIds = soln.ClaimedCapabilityReview.Select(cce => cce.PreviousId).Where(id => id != null);
          return evidencePrevIds.All(prevId => evidenceIds.Contains(prevId));
        })
        .WithMessage("ClaimedCapabilityReview previous version must belong to solution");
    }

    public void ClaimedStandardReviewPreviousVersionMustBelongToSolution()
    {
      RuleFor(x => x)
        .Must(soln =>
        {
          var evidenceIds = soln.ClaimedStandardReview.Select(cce => cce.Id);
          var evidencePrevIds = soln.ClaimedStandardReview.Select(cce => cce.PreviousId).Where(id => id != null);
          return evidencePrevIds.All(prevId => evidenceIds.Contains(prevId));
        })
        .WithMessage("ClaimedStandardReview previous version must belong to solution");
    }
  }
}
