using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM
{
  public abstract class ReviewsDatastoreBase<T> : DatastoreBase<T>, IReviewsDatastore<ReviewsBase> where T : ReviewsBase
  {
    protected abstract string ResourceBase { get; }

    public ReviewsDatastoreBase(
      IRestClientFactory crmFactory, 
      ILogger<DatastoreBase<T>> logger, 
      ISyncPolicyFactory policy,
      IConfiguration config) : 
      base(crmFactory, logger, policy, config)
    {
    }

    public IEnumerable<IEnumerable<T>> ByEvidence(string evidenceId)
    {
      return GetInternal(() =>
      {
        var request = GetAllRequest($"{ResourceBase}/ByEvidence/{evidenceId}");
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

    public T Create(T review)
    {
      return GetInternal(() =>
      {
        review.Id = UpdateId(review.Id);
        var request = GetPostRequest($"{ResourceBase}", review);
        var retval = GetResponse<T>(request);

        return retval;
      });
    }

    public void Delete(T review)
    {
      GetInternal(() =>
      {
        var request = GetDeleteRequest($"{ResourceBase}", review);
        var resp = GetRawResponse(request);

        return 0;
      });
    }

    IEnumerable<IEnumerable<ReviewsBase>> IReviewsDatastore<ReviewsBase>.ByEvidence(string evidenceId)
    {
      return ByEvidence(evidenceId);
    }

    ReviewsBase IReviewsDatastore<ReviewsBase>.ById(string id)
    {
      return ById(id);
    }

    ReviewsBase IReviewsDatastore<ReviewsBase>.Create(ReviewsBase review)
    {
      return Create((T)review);
    }

    void IReviewsDatastore<ReviewsBase>.Delete(ReviewsBase review)
    {
      Delete((T)review);
    }
  }
}
