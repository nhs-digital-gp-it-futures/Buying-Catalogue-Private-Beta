using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class ClaimsValidatorBase<T> : ValidatorBase<T>, IClaimsValidator<T> where T : ClaimsBase
  {
    protected readonly IClaimsDatastore<T> _claimDatastore;
    protected readonly IContactsDatastore _contactsDatastore;
    protected readonly ISolutionsDatastore _solutionsDatastore;

    public ClaimsValidatorBase(
      IHttpContextAccessor context,
      ILogger<ClaimsValidatorBase<T>> logger,
      IClaimsDatastore<T> claimDatastore,
      IContactsDatastore contactsDatastore,
      ISolutionsDatastore solutionsDatastore) :
      base(context, logger)
    {
      _claimDatastore = claimDatastore;
      _contactsDatastore = contactsDatastore;
      _solutionsDatastore = solutionsDatastore;

      RuleSet(nameof(IClaimsLogic<T>.Create), () =>
      {
        MustBeValidSolutionId();
        MustBeSameOrganisation();
        MustBePending();
        MustBeValidOwnerId();
        OwnerMustBeSameOrganisation();
      });

      RuleSet(nameof(IClaimsLogic<T>.Update), () =>
      {
        MustBeValidId();
        MustBeValidSolutionId();
        MustBeSameSolution();
        MustBeSameOrganisation();
        MustBeValidStatusTransition();
        MustBeValidOwnerId();
        OwnerMustBeSameOrganisation();
      });

      RuleSet(nameof(IClaimsLogic<T>.Delete), () =>
      {
        MustBeValidId();
        MustBeValidSolutionId();
        MustBeSameOrganisation();
        MustBePending();
      });
    }

    public void MustBeValidSolutionId()
    {
      RuleFor(x => x.SolutionId)
        .NotNull()
        .Must(solnId => Guid.TryParse(solnId, out _))
        .WithMessage("Invalid SolutionId");
    }

    public void MustBeValidId()
    {
      RuleFor(x => x.Id)
        .NotNull()
        .Must(id => Guid.TryParse(id, out _))
        .WithMessage("Invalid Id");
    }

    public abstract void MustBePending();
    public abstract void MustBeValidStatusTransition();

    public void MustBeSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var orgId = _context.OrganisationId();
          var claim = _claimDatastore.ById(x.Id);
          var claimSoln = _solutionsDatastore.ById(claim?.SolutionId ?? x.SolutionId);
          return claimSoln?.OrganisationId == orgId;
        })
        .WithMessage("Cannot create/change claim for other organisation");
    }

    public void MustBeSameSolution()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var claim = _claimDatastore.ById(x.Id);
          return x.SolutionId == claim?.SolutionId;
        })
        .WithMessage("Cannot transfer claim between solutions");
    }

    public void OwnerMustBeSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var orgId = _context.OrganisationId();
          var claimContact = _contactsDatastore.ById(x.OwnerId);
          return claimContact?.OrganisationId == orgId;
        })
        .WithMessage("Contact must be from organisation");
    }

    public void MustBeValidOwnerId()
    {
      RuleFor(x => x.OwnerId)
        .NotNull()
        .Must(id => Guid.TryParse(id, out _))
        .When(x => x.OwnerId != null)
        .WithMessage("Invalid OwnerId");
    }
  }
}
