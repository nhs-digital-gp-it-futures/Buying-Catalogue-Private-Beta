using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsLogic : LogicBase, IStandardsLogic
  {
    private readonly IStandardsDatastore _datastore;
    private readonly IStandardsFilter _filter;

    public StandardsLogic(
      IStandardsDatastore datastore,
      IHttpContextAccessor context,
      IStandardsFilter filter) :
      base(context)
    {
      _datastore = datastore;
      _filter = filter;
    }

    public IEnumerable<Standards> ByCapability(string capabilityId, bool isOptional)
    {
      return _filter.Filter(_datastore.ByCapability(capabilityId, isOptional));
    }

    public IEnumerable<Standards> ByFramework(string frameworkId)
    {
      return _filter.Filter(_datastore.ByFramework(frameworkId));
    }

    public Standards ById(string id)
    {
      return _filter.Filter(new[] { _datastore.ById(id) }).SingleOrDefault();
    }

    public IEnumerable<Standards> ByIds(IEnumerable<string> ids)
    {
      return _filter.Filter(_datastore.ByIds(ids));
    }

    public IEnumerable<Standards> GetAll()
    {
      return _filter.Filter(_datastore.GetAll());
    }
  }
}
