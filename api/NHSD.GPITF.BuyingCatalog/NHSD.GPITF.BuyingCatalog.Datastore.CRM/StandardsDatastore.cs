using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class StandardsDatastore : CachedDatastore<Standards>, IStandardsDatastore
  {
    public StandardsDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<StandardsDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config,
      IDatastoreCache cache) :
      base(crmConnectionFactory, logger, policy, config, cache)
    {
    }

    private string ResourceBase { get; } = "/Standards";

    public IEnumerable<Standards> ByCapability(string capabilityId, bool isOptional)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByCapability/{capabilityId}");
        request.AddQueryParameter("isOptional", isOptional.ToString().ToLowerInvariant());
        var retval = GetResponse<PaginatedList<Standards>>(request);

        return retval.Items;
      });
    }

    public IEnumerable<Standards> ByFramework(string frameworkId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByFramework/{frameworkId}");
        var retval = GetResponse<PaginatedList<Standards>>(request);

        return retval.Items;
      });
    }

    public Standards ById(string id)
    {
      return GetInternal(() =>
      {
        return GetAll().SingleOrDefault(x => x.Id == id);
      });
    }

    public IEnumerable<Standards> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        var request = GetAllPostRequest($"{ResourceBase}/ByIds", ids);
        var retval = GetResponse<IEnumerable<Standards>>(request);

        return retval;
      });
    }

    public IEnumerable<Standards> GetAll()
    {
      return GetInternal(() =>
      {
        return GetAll($"{ResourceBase}");
      });
    }
  }
}
