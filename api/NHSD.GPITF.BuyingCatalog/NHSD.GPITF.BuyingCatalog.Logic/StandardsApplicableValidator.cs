using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableValidator : ClaimsValidatorBase<StandardsApplicable>, IStandardsApplicableValidator
  {
    public StandardsApplicableValidator(
      IHttpContextAccessor context,
      ILogger<StandardsApplicableValidator> logger,
      IStandardsApplicableDatastore claimDatastore,
      IContactsDatastore contactsDatastore,
      ISolutionsDatastore solutionsDatastore) :
      base(context, logger, claimDatastore, contactsDatastore, solutionsDatastore)
    {
    }

    public override void MustBePending()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          return _context.HasRole(Roles.Supplier) &&
            (x.Status == StandardsApplicableStatus.NotStarted ||
            x.Status == StandardsApplicableStatus.Draft);
        })
        .WithMessage("Only supplier can delete a draft claim");
    }

    public override void MustBeValidStatusTransition()
    {
      RuleFor(x => x)
        .Must(x =>
        {
          var claim = _claimDatastore.ById(x.Id);
          if (claim == null)
          {
            return false;
          }
          var oldStatus = claim.Status;
          var newStatus = x.Status;
          return ValidStatusTransitions(_context).Any(
            trans =>
              trans.OldStatus == oldStatus &&
              trans.NewStatus == newStatus &&
              trans.HasValidRole);
        })
        .WithMessage("Invalid Status transition");
    }

    private static IEnumerable<(StandardsApplicableStatus OldStatus, StandardsApplicableStatus NewStatus, bool HasValidRole)> ValidStatusTransitions(IHttpContextAccessor context)
    {
      yield return (StandardsApplicableStatus.NotStarted, StandardsApplicableStatus.Draft, context.HasRole(Roles.Supplier));
      yield return (StandardsApplicableStatus.Draft, StandardsApplicableStatus.Submitted, context.HasRole(Roles.Supplier));
      yield return (StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Remediation, context.HasRole(Roles.Admin));
      yield return (StandardsApplicableStatus.Remediation, StandardsApplicableStatus.Submitted, context.HasRole(Roles.Supplier));
      yield return (StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Rejected, context.HasRole(Roles.Admin));
      yield return (StandardsApplicableStatus.Submitted, StandardsApplicableStatus.Approved, context.HasRole(Roles.Admin));
      yield return (StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedFirstOfType, context.HasRole(Roles.Admin));
      yield return (StandardsApplicableStatus.Submitted, StandardsApplicableStatus.ApprovedPartial, context.HasRole(Roles.Admin));
    }
  }
}
