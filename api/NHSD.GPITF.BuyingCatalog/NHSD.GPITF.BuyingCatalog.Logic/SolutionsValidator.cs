using FluentValidation;
using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class SolutionsValidator : ValidatorBase<Solutions>, ISolutionsValidator
  {
    private readonly ISolutionsDatastore _solutionDatastore;
    private readonly IOrganisationsDatastore _organisationDatastore;
    private readonly IHostingEnvironment _env;

    public SolutionsValidator(
      IHttpContextAccessor context,
      ILogger<SolutionsValidator> logger,
      ISolutionsDatastore solutionDatastore,
      IOrganisationsDatastore organisationDatastore,
      IHostingEnvironment env) :
      base(context, logger)
    {
      _solutionDatastore = solutionDatastore;
      _organisationDatastore = organisationDatastore;
      _env = env;

      RuleSet(nameof(ISolutionsLogic.Update), () =>
      {
        MustBeValidId();
        MustBeValidOrganisationId();
        MustBeSameOrganisation();
        MustBeFromSameOrganisationOrAdmin();
        MustBeValidStatusTransition();
        MustBeCurrentVersion();
        PreviousVersionMustBeFromSameOrganisation();
        MustBePendingToChangeName();
        MustBePendingToChangeVersion();
      });

      RuleSet(nameof(ISolutionsLogic.Create), () =>
      {
        MustBeValidOrganisationId();
        MustBeFromSameOrganisationOrAdmin();
        PreviousVersionMustBeFromSameOrganisation();
        MustBePending();
      });

      RuleSet(nameof(ISolutionsLogic.Delete), () =>
      {
        MustBeDevelopment();
        MustBeAdminOrSupplier();
        MustBeValidId();
        MustBeValidOrganisationId();
        MustBeCurrentVersion();
        PreviousVersionMustBeFromSameOrganisation();
      });
    }

    public void MustBeValidId()
    {
      RuleFor(x => x.Id)
        .NotNull()
        .Must(id => Guid.TryParse(id, out _))
        .WithMessage("Invalid Id");
    }

    public void MustBeValidOrganisationId()
    {
      RuleFor(x => x.OrganisationId)
        .NotNull()
        .Must(orgId => Guid.TryParse(orgId, out _))
        .WithMessage("Invalid OrganisationId");
    }

    public void MustBeValidStatusTransition()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          /// NOTE:  null solution check is not quite correct
          /// as this would result in a FK exception if we let it through
          /// but it is good enough for the moment
          var soln = _solutionDatastore.ById(x.Id);
          if (soln == null)
          {
            return false;
          }
          var oldStatus = soln.Status;
          var newStatus = x.Status;
          return ValidStatusTransitions(_context).Any(
            trans =>
              trans.OldStatus == oldStatus &&
              trans.NewStatus == newStatus &&
              trans.HasValidRole);
        })
        .WithMessage("Invalid Status transition");
    }

    public void MustBeSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          /// NOTE:  null solution check is not quite correct
          /// as this would result in a FK exception if we let it through
          /// but it is good enough for the moment
          var soln = _solutionDatastore.ById(x.Id);
          return soln != null && x.OrganisationId == soln.OrganisationId;
        })
        .WithMessage("Cannot transfer solutions between organisations");
    }

    public void MustBeFromSameOrganisationOrAdmin()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var orgId = _context.OrganisationId();
          return x.OrganisationId == orgId;
        })
        .When(x => !_context.HasRole(Roles.Admin))
        .WithMessage("Must be from same organisation");
    }

    public void MustBeCurrentVersion()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var solns = _solutionDatastore.ByOrganisation(x.OrganisationId);
          return solns.Select(soln => soln.Id).Contains(x.Id);
        })
        .WithMessage("Can only change current version");
    }

    public void PreviousVersionMustBeFromSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          if (x.PreviousId == null)
          {
            return true;
          }
          var soln = _solutionDatastore.ById(x.PreviousId);
          return soln != null && soln.OrganisationId == x.OrganisationId;
        })
        .WithMessage("Previous version must be from same organisation");
    }

    public void MustBePending()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          return x.Status == SolutionStatus.Draft;
        })
        .WithMessage("Status must be Draft");
    }

    public void MustBeDevelopment()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          return _env.IsDevelopment();
        })
        .WithMessage("Only available in Development environment");
    }

    public void MustBePendingToChangeName()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var soln = _solutionDatastore.ById(x.Id);
          return
            x.Status == SolutionStatus.Draft ||
            x.Name == soln.Name;
        })
        .WithMessage("Can only change name in Draft");
    }

    public void MustBePendingToChangeVersion()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var soln = _solutionDatastore.ById(x.Id);
          return
            x.Status == SolutionStatus.Draft ||
            x.Version == soln.Version;
        })
        .WithMessage("Can only change version in Draft");
    }

    private static IEnumerable<(SolutionStatus OldStatus, SolutionStatus NewStatus, bool HasValidRole)> ValidStatusTransitions(IHttpContextAccessor context)
    {
      yield return (SolutionStatus.Draft, SolutionStatus.Draft, context.HasRole(Roles.Supplier));
      yield return (SolutionStatus.Draft, SolutionStatus.Registered, context.HasRole(Roles.Supplier));
      yield return (SolutionStatus.Registered, SolutionStatus.Registered, context.HasRole(Roles.Supplier));
      yield return (SolutionStatus.Registered, SolutionStatus.CapabilitiesAssessment, context.HasRole(Roles.Supplier));
      yield return (SolutionStatus.CapabilitiesAssessment, SolutionStatus.Failed, context.HasRole(Roles.Admin));
      yield return (SolutionStatus.CapabilitiesAssessment, SolutionStatus.StandardsCompliance, context.HasRole(Roles.Admin));
      yield return (SolutionStatus.CapabilitiesAssessment, SolutionStatus.StandardsCompliance, context.HasRole(Roles.Supplier));
      yield return (SolutionStatus.StandardsCompliance, SolutionStatus.StandardsCompliance, context.HasRole(Roles.Supplier));
      yield return (SolutionStatus.StandardsCompliance, SolutionStatus.Failed, context.HasRole(Roles.Admin));
      yield return (SolutionStatus.StandardsCompliance, SolutionStatus.FinalApproval, context.HasRole(Roles.Admin));
      yield return (SolutionStatus.FinalApproval, SolutionStatus.SolutionPage, context.HasRole(Roles.Admin));
      yield return (SolutionStatus.SolutionPage, SolutionStatus.Approved, context.HasRole(Roles.Admin));
    }
  }
}
