using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;

namespace NHSD.GPITF.BuyingCatalog.Logic.Porcelain
{
  public sealed class CapabilityMappingsLogic : LogicBase, ICapabilityMappingsLogic
  {
    private readonly ICapabilityMappingsDatastore _datastore;

    public CapabilityMappingsLogic(ICapabilityMappingsDatastore datastore, IHttpContextAccessor context) :
      base(context)
    {
      _datastore = datastore;
    }

    public CapabilityMappings GetAll()
    {
      return _datastore.GetAll();
    }
  }
}
