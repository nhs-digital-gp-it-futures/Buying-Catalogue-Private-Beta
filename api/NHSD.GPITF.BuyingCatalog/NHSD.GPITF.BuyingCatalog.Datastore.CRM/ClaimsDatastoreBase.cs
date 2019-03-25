using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public abstract class ClaimsDatastoreBase<T> : DatastoreBase<T>, IClaimsDatastore<ClaimsBase> where T : ClaimsBase
  {
    protected abstract string ResourceBase { get; }

    public ClaimsDatastoreBase(
      IRestClientFactory crmFactory, 
      ILogger<DatastoreBase<T>> logger, 
      ISyncPolicyFactory policy,
      IConfiguration config) : 
      base(crmFactory, logger, policy, config)
    {
    }

    public T ById(string id)
    {
      return GetInternal(() =>
      {
        var request = GetRequest($"{ResourceBase}/ById/{id}");
        var retval = GetResponse<T>(request);

        return retval;
      });
    }

    public IEnumerable<T> BySolution(string solutionId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/BySolution/{solutionId}");
        var retval = GetResponse<PaginatedList<T>>(request);

        return retval.Items;
      });
    }

    public T Create(T claim)
    {
      return GetInternal(() =>
      {
        claim.Id = UpdateId(claim.Id);
        var request = GetPostRequest($"{ResourceBase}", claim);
        var retval = GetResponse<T>(request);

        return retval;
      });
    }

    public void Delete(T claim)
    {
      GetInternal(() =>
      {
        var request = GetDeleteRequest($"{ResourceBase}", claim);
        var resp = GetRawResponse(request);

        return 0;
      });
    }

    public void Update(T claim)
    {
      GetInternal(() =>
      {
        var request = GetPutRequest($"{ResourceBase}", claim);
        var resp = GetRawResponse(request);

        return 0;
      });
    }

    ClaimsBase IClaimsDatastore<ClaimsBase>.ById(string id)
    {
      return ById(id);
    }

    IEnumerable<ClaimsBase> IClaimsDatastore<ClaimsBase>.BySolution(string solutionId)
    {
      return BySolution(solutionId);
    }

    ClaimsBase IClaimsDatastore<ClaimsBase>.Create(ClaimsBase claim)
    {
      return Create((T)claim);
    }

    void IClaimsDatastore<ClaimsBase>.Delete(ClaimsBase claim)
    {
      Delete((T)claim);
    }

    void IClaimsDatastore<ClaimsBase>.Update(ClaimsBase claim)
    {
      Update((T)claim);
    }
  }
}
