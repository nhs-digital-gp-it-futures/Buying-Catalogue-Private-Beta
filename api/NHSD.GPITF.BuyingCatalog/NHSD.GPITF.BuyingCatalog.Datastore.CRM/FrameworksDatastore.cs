using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class FrameworksDatastore : DatastoreBase<Frameworks>, IFrameworksDatastore
  {
    public FrameworksDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<FrameworksDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmConnectionFactory, logger, policy, config)
    {
    }

    private string ResourceBase { get; } = "/Frameworks";

    public IEnumerable<Frameworks> ByCapability(string capabilityId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByCapability/{capabilityId}");
        var retval = GetResponse<PaginatedList<Frameworks>>(request);

        return retval.Items;
      });
    }

    public Frameworks ById(string id)
    {
      return GetInternal(() =>
      {
        var request = GetRequest($"{ResourceBase}/ById/{id}");
        var retval = GetResponse<Frameworks>(request);

        return retval;
      });
    }

    public IEnumerable<Frameworks> BySolution(string solutionId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/BySolution/{solutionId}");
        var retval = GetResponse<PaginatedList<Frameworks>>(request);

        return retval.Items;
      });
    }

    public IEnumerable<Frameworks> ByStandard(string standardId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByStandard/{standardId}");
        var retval = GetResponse<PaginatedList<Frameworks>>(request);

        return retval.Items;
      });
    }

    public IEnumerable<Frameworks> GetAll()
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}");
        var retval = GetResponse<PaginatedList<Frameworks>>(request);

        return retval.Items;
      });
    }
  }
}
