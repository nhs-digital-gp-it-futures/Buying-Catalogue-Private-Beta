using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public abstract class EvidenceDatastoreBase<T> : DatastoreBase<T>, IEvidenceDatastore<EvidenceBase> where T : EvidenceBase
  {
    protected abstract string ResourceBase { get; }

    public EvidenceDatastoreBase(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<T>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config) :
      base(crmFactory, logger, policy, config)
    {
    }

    public IEnumerable<IEnumerable<T>> ByClaim(string claimId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByClaim/{claimId}");
        var retval = GetResponse<PaginatedList<IEnumerable<T>>>(request);

        return retval.Items;
      });
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

    public T Create(T evidence)
    {
      return GetInternal(() =>
      {
        evidence.Id = UpdateId(evidence.Id);
        var request = GetPostRequest($"{ResourceBase}", evidence);
        var retval = GetResponse<T>(request);

        return retval;
      });
    }

    IEnumerable<IEnumerable<EvidenceBase>> IEvidenceDatastore<EvidenceBase>.ByClaim(string claimId)
    {
      return ByClaim(claimId);
    }

    EvidenceBase IEvidenceDatastore<EvidenceBase>.ById(string id)
    {
      return ById(id);
    }

    EvidenceBase IEvidenceDatastore<EvidenceBase>.Create(EvidenceBase evidence)
    {
      return Create((T)evidence);
    }
  }
}
