using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class EvidenceFilterBase<T> : FilterBase<T>, IFilter<T> where T : IEnumerable<EvidenceBase>
  {
    private readonly IClaimsDatastore<ClaimsBase> _claimDatastore;
    private readonly ISolutionsDatastore _solutionDatastore;

    public EvidenceFilterBase(
      IClaimsDatastore<ClaimsBase> claimDatastore,
      ISolutionsDatastore solutionDatastore,
      IHttpContextAccessor context) :
      base(context)
    {
      _claimDatastore = claimDatastore;
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
      // Buyer: nothing
      return default(T);
    }

    public T FilterForSupplier(T input)
    {
      // Supplier: only own Claims
      foreach (var evidence in input)
      {
        var claim = _claimDatastore.ById(evidence.ClaimId);
        var soln = _solutionDatastore.ById(claim.SolutionId);
        if (_context.OrganisationId() != soln?.OrganisationId)
        {
          return default(T);
        }
      }

      return input;
    }
  }
}
