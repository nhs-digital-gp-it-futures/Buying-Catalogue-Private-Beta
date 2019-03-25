using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class TechnicalContactsFilter : FilterBase<TechnicalContacts>, ITechnicalContactsFilter
  {
    private readonly ISolutionsDatastore _solutionDatastore;

    public TechnicalContactsFilter(
      IHttpContextAccessor context,
      ISolutionsDatastore solutionDatastore) :
      base(context)
    {
      _solutionDatastore = solutionDatastore;
    }

    public override TechnicalContacts Filter(TechnicalContacts input)
    {
      if (_context.HasRole(Roles.Admin) ||
        _context.HasRole(Roles.Buyer))
      {
        return input;
      }

      // Supplier: only own TechnicalContacts
      var soln = _solutionDatastore.ById(input.SolutionId);
      return _context.OrganisationId() == soln.OrganisationId ? input : null;
    }
  }
}
