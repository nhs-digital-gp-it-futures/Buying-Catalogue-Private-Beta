using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class FrameworksLogic : LogicBase, IFrameworksLogic
  {
    private readonly IFrameworksDatastore _datastore;
    private readonly IFrameworksFilter _filter;

    public FrameworksLogic(
      IFrameworksDatastore datastore,
      IHttpContextAccessor context,
      IFrameworksFilter filter) :
      base(context)
    {
      _datastore = datastore;
      _filter = filter;
    }

    public IEnumerable<Frameworks> ByCapability(string capabilityId)
    {
      return _filter.Filter(_datastore.ByCapability(capabilityId));
    }

    public IEnumerable<Frameworks> ByStandard(string standardId)
    {
      return _filter.Filter(_datastore.ByStandard(standardId));
    }

    public Frameworks ById(string id)
    {
      return _filter.Filter(new[] { _datastore.ById(id) }).SingleOrDefault();
    }

    public IEnumerable<Frameworks> BySolution(string solutionId)
    {
      return _filter.Filter(_datastore.BySolution(solutionId));
    }

    public IEnumerable<Frameworks> GetAll()
    {
      return _filter.Filter(_datastore.GetAll());
    }
  }
}
