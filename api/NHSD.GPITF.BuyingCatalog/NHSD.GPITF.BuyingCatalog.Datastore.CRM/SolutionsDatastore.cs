using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class SolutionsDatastore : DatastoreBase<Solutions>, ISolutionsDatastore
  {
    public SolutionsDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<SolutionsDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmConnectionFactory, logger, policy, config)
    {
    }

    private string ResourceBase { get; } = "/Solutions";

    public IEnumerable<Solutions> ByFramework(string frameworkId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByFramework/{frameworkId}");
        var retval = GetResponse<PaginatedList<Solutions>>(request);

        return retval.Items;
      });
    }

    public Solutions ById(string id)
    {
      return GetInternal(() =>
      {
        var request = GetRequest($"{ResourceBase}/ById/{id}");
        var retval = GetResponse<Solutions>(request);

        return retval;
      });
    }

    public IEnumerable<Solutions> ByOrganisation(string organisationId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByOrganisation/{organisationId}");
        var retval = GetResponse<PaginatedList<Solutions>>(request);

        return retval.Items;
      });
    }

    public Solutions Create(Solutions solution)
    {
      return GetInternal(() =>
      {
        solution.Id = UpdateId(solution.Id);
        var request = GetPostRequest($"{ResourceBase}", solution);
        var retval = GetResponse<Solutions>(request);

        return retval;
      });
    }

    public void Update(Solutions solution)
    {
      GetInternal(() =>
      {
        var request = GetPutRequest($"{ResourceBase}", solution);
        var resp = GetRawResponse(request);

        return 0;
      });
    }

    public void Delete(Solutions solution)
    {
      GetInternal(() =>
      {
        var request = GetDeleteRequest($"{ResourceBase}", solution);
        var resp = GetRawResponse(request);

        return 0;
      });
    }
  }
}
