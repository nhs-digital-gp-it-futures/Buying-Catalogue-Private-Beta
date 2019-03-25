using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesImplementedValidator : ClaimsValidatorBase<CapabilitiesImplemented>, ICapabilitiesImplementedValidator
  {
    public CapabilitiesImplementedValidator(
      IHttpContextAccessor context,
      ILogger<CapabilitiesImplementedValidator> logger,
      ICapabilitiesImplementedDatastore claimDatastore,
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
            x.Status == CapabilitiesImplementedStatus.Draft;
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

    private static IEnumerable<(CapabilitiesImplementedStatus OldStatus, CapabilitiesImplementedStatus NewStatus, bool HasValidRole)> ValidStatusTransitions(IHttpContextAccessor context)
    {
      yield return (CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Draft, context.HasRole(Roles.Supplier));
      yield return (CapabilitiesImplementedStatus.Draft, CapabilitiesImplementedStatus.Submitted, context.HasRole(Roles.Supplier));
      yield return (CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Remediation, context.HasRole(Roles.Admin));
      yield return (CapabilitiesImplementedStatus.Remediation, CapabilitiesImplementedStatus.Submitted, context.HasRole(Roles.Supplier));
      yield return (CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Approved, context.HasRole(Roles.Admin));
      yield return (CapabilitiesImplementedStatus.Submitted, CapabilitiesImplementedStatus.Rejected, context.HasRole(Roles.Admin));
    }
  }
}
