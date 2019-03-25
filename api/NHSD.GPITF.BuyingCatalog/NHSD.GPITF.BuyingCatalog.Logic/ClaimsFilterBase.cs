using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class ClaimsFilterBase<T> : FilterBase<T>, IClaimsFilter<T> where T : ClaimsBase
  {
    private readonly ISolutionsDatastore _solutionDatastore;

    public ClaimsFilterBase(
      IHttpContextAccessor context,
      ISolutionsDatastore solutionDatastore) :
      base(context)
    {
      _solutionDatastore = solutionDatastore;
    }

    protected virtual T FilterSpecific(T input)
    {
      return input;
    }

    public override T Filter(T input)
    {
      if (_context.HasRole(Roles.Admin))
      {
        input = FilterForAdmin(input);
      }

      if (_context.HasRole(Roles.Buyer))
      {
        input = FilterForBuyer(input);
      }

      if (_context.HasRole(Roles.Supplier))
      {
        input = FilterForSupplier(input);
      }

      return FilterSpecific(input);
    }

    public T FilterForAdmin(T input)
    {
      // Admin: everything
      return input;
    }

    public T FilterForBuyer(T input)
    {
      // Buyer: everything
      return input;
    }

    public T FilterForSupplier(T input)
    {
      // Supplier: only own Claims
      var soln = _solutionDatastore.ById(input.SolutionId);
      return _context.OrganisationId() == soln?.OrganisationId ? input : null;
    }
  }
}
