using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class CapabilitiesLogic : LogicBase, ICapabilitiesLogic
  {
    private readonly ICapabilitiesDatastore _datastore;
    private readonly ICapabilitiesFilter _filter;

    public CapabilitiesLogic(
      ICapabilitiesDatastore datastore, 
      IHttpContextAccessor context,
      ICapabilitiesFilter filter) :
      base(context)
    {
      _datastore = datastore;
      _filter = filter;
    }

    public IEnumerable<Capabilities> ByFramework(string frameworkId)
    {
      return _filter.Filter(_datastore.ByFramework(frameworkId));
    }

    public Capabilities ById(string id)
    {
      return _filter.Filter(new[] { _datastore.ById(id) }).SingleOrDefault();
    }

    public IEnumerable<Capabilities> ByIds(IEnumerable<string> ids)
    {
      return _filter.Filter(_datastore.ByIds(ids));
    }

    public IEnumerable<Capabilities> ByStandard(string standardId, bool isOptional)
    {
      return _filter.Filter(_datastore.ByStandard(standardId, isOptional));
    }

    public IEnumerable<Capabilities> GetAll()
    {
      return _filter.Filter(_datastore.GetAll());
    }
  }
}
