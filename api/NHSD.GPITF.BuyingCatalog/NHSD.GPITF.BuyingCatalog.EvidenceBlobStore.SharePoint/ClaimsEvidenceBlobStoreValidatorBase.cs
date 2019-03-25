using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public abstract class ClaimsEvidenceBlobStoreValidatorBase : EvidenceBlobStoreValidator
  {
    protected readonly IClaimsDatastore<ClaimsBase> _claimsDatastore;
    protected readonly ISolutionsDatastore _solutionsDatastore;

    public ClaimsEvidenceBlobStoreValidatorBase(
      IHttpContextAccessor context,
      ILogger<ClaimsEvidenceBlobStoreValidatorBase> logger,
      ISolutionsDatastore solutionsDatastore,
      IClaimsDatastore<ClaimsBase> claimsDatastore) :
      base(context, logger)
    {
      _solutionsDatastore = solutionsDatastore;
      _claimsDatastore = claimsDatastore;

      // claimId
      RuleSet(nameof(IEvidenceBlobStoreLogic.AddEvidenceForClaim), () =>
      {
        MustBeValidClaim();
        MustBeSameOrganisation();
      });

      // claimId
      RuleSet(nameof(IEvidenceBlobStoreLogic.GetFileStream), () =>
      {
        MustBeValidClaim();
        MustBeSameOrganisation();
      });

      // claimId
      RuleSet(nameof(IEvidenceBlobStoreLogic.EnumerateFolder), () =>
      {
        MustBeValidClaim();
        MustBeSameOrganisation();
      });

      // solutionId
      RuleSet(nameof(IEvidenceBlobStoreLogic.EnumerateClaimFolderTree), () =>
      {
        MustBeSameOrganisationById();
      });
    }

    public void MustBeValidClaim()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var claim = _claimsDatastore.ById(x);
          return claim != null;
        })
        .WithMessage("Could not find claim");
    }

    public void MustBeSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var orgId = _context.OrganisationId();
          var claim = _claimsDatastore.ById(x);
          var claimSoln = _solutionsDatastore.ById(claim?.SolutionId ?? Guid.NewGuid().ToString());
          return claimSoln?.OrganisationId == orgId;
        })
        .When(x => _context.HasRole(Roles.Supplier))
        .WithMessage("Cannot add/see evidence for other organisation");
    }

    public void MustBeSameOrganisationById()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var orgId = _context.OrganisationId();
          var claimSoln = _solutionsDatastore.ById(x);
          return claimSoln?.OrganisationId == orgId;
        })
        .When(x => _context.HasRole(Roles.Supplier))
        .WithMessage("Cannot add/see evidence for other organisation");
    }
  }
}
