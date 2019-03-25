using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public sealed class TechnicalContactsDatastore : DatastoreBase<TechnicalContacts>, ITechnicalContactsDatastore
  {
    public TechnicalContactsDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<TechnicalContactsDatastore> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmConnectionFactory, logger, policy, config)
    {
    }

    private string ResourceBase { get; } = "/TechnicalContacts";

    public IEnumerable<TechnicalContacts> BySolution(string solutionId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/BySolution/{solutionId}");
        var retval = GetResponse<PaginatedList<TechnicalContacts>>(request);

        return retval.Items;
      });
    }

    public TechnicalContacts Create(TechnicalContacts techCont)
    {
      return GetInternal(() =>
      {
        techCont.Id = UpdateId(techCont.Id);
        var request = GetPostRequest($"{ResourceBase}", techCont);
        var retval = GetResponse<TechnicalContacts>(request);

        return retval;
      });
    }

    public void Delete(TechnicalContacts techCont)
    {
      GetInternal(() =>
      {
        var request = GetDeleteRequest($"{ResourceBase}", techCont);
        var resp = GetRawResponse(request);

        return 0;
      });
    }

    public void Update(TechnicalContacts techCont)
    {
      GetInternal(() =>
      {
        var request = GetPutRequest($"{ResourceBase}", techCont);
        var resp = GetRawResponse(request);

        return 0;
      });
    }
  }
}
