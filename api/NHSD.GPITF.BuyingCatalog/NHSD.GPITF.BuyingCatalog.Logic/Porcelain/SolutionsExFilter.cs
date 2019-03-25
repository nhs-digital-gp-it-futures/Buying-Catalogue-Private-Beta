using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;

namespace NHSD.GPITF.BuyingCatalog.Logic.Porcelain
{
  public sealed class SolutionsExFilter : FilterBase<SolutionEx>, ISolutionsExFilter
  {
    private readonly ISolutionsFilter _solutionsFilter;

    public SolutionsExFilter(
      IHttpContextAccessor context,
      ISolutionsFilter solutionsFilter) :
      base(context)
    {
      _solutionsFilter = solutionsFilter;
    }

    public override SolutionEx Filter(SolutionEx input)
    {
      return _solutionsFilter.Filter(new[] { input?.Solution }) != null ? input : null;
    }
  }
}
