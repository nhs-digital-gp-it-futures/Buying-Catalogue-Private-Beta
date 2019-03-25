using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class LinkManagerDatastore : DatastoreBase<object>, ILinkManagerDatastore
  {
    public LinkManagerDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<LinkManagerDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmConnectionFactory, logger, policy, config)
    {
    }

    private string ResourceBase { get; } = "/LinkManager";

    public void FrameworkSolutionCreate(string frameworkId, string solutionId)
    {
      GetInternal(() =>
      {
        var request = GetPostRequest($"{ResourceBase}/FrameworkSolution/Create/{frameworkId}/{solutionId}", null);
        var resp = GetRawResponse(request);

        return 0;
      });
    }
  }
}
