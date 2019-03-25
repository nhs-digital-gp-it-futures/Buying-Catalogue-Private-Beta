using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class CapabilitiesDatastore : CachedDatastore<Capabilities>, ICapabilitiesDatastore
  {
    public CapabilitiesDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<CapabilitiesDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config,
      IDatastoreCache cache) :
      base(crmConnectionFactory, logger, policy, config, cache)
    {
    }

    private string ResourceBase { get; } = "/Capabilities";

    public IEnumerable<Capabilities> ByFramework(string frameworkId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByFramework/{frameworkId}");
        var retval = GetResponse<PaginatedList<Capabilities>>(request);

        return retval.Items;
      });
    }

    public Capabilities ById(string id)
    {
      return GetInternal(() =>
      {
        return GetAll().SingleOrDefault(x => x.Id == id);
      });
    }

    public IEnumerable<Capabilities> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        var request = GetPostRequest($"{ResourceBase}/ByIds", ids);
        var retval = GetResponse<IEnumerable<Capabilities>>(request);

        return retval;
      });
    }

    public IEnumerable<Capabilities> ByStandard(string standardId, bool isOptional)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByStandard/{standardId}");
        request.AddQueryParameter("isOptional", isOptional.ToString().ToLowerInvariant());
        var retval = GetResponse<PaginatedList<Capabilities>>(request);

        return retval.Items;
      });
    }

    public IEnumerable<Capabilities> GetAll()
    {
      return GetInternal(() =>
      {
        return GetAll($"{ResourceBase}");
      });
    }
  }
}
