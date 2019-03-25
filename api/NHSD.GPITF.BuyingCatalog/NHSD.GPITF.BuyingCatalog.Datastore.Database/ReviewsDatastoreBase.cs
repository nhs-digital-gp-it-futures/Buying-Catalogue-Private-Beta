using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Reflection;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public abstract class ReviewsDatastoreBase<T> : CommonTableExpressionDatastoreBase<T>, IReviewsDatastore<ReviewsBase> where T : ReviewsBase
  {
    public ReviewsDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ReviewsDatastoreBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<IEnumerable<T>> ByEvidence(string evidenceId)
    {
      return GetInternal(() =>
      {
        var table = typeof(T).GetCustomAttribute<TableAttribute>(true);
        var chains = new List<IEnumerable<T>>();
        var sqlAllCurrent = $@"
-- select all current versions
select rev.* from {table.Name} rev where Id not in 
(
  select PreviousId from {table.Name} where PreviousId is not null
) and
EvidenceId = @evidenceId
";
        var allCurrent = _dbConnection.Value.Query<T>(sqlAllCurrent, new { evidenceId });
        foreach (var current in allCurrent)
        {
          var sqlCurrent = $@"
-- get all previous versions from a specified (CurrentId) version
with recursive Links(CurrentId, Id, PreviousId, EvidenceId, CreatedById, CreatedOn, OriginalDate, Message) as (
  select
    Id, Id, PreviousId, EvidenceId, CreatedById, CreatedOn, OriginalDate, Message
  from {table.Name}
  where PreviousId is null
  
  union all
  select
    Id, Id, PreviousId, EvidenceId, CreatedById, CreatedOn, OriginalDate, Message
  from {table.Name} 
  where PreviousId is not null
  
  union all
  select
    Links.CurrentId,
    {table.Name}.Id,
    {table.Name}.PreviousId,
    {table.Name}.EvidenceId,
    {table.Name}.CreatedById,
    {table.Name}.CreatedOn,
    {table.Name}.OriginalDate,
    {table.Name}.Message
  from Links
  join {table.Name}
  on Links.PreviousId = {table.Name}.Id
)
  select Links.Id, Links.PreviousId, Links.EvidenceId, Links.CreatedById, Links.CreatedOn, Links.OriginalDate, Links.Message
  from Links
  where CurrentId = @currentId;
";
          var amendedSql = AmendCommonTableExpression(sqlCurrent);
          var chain = _dbConnection.Value.Query<T>(amendedSql, new { currentId = current.Id });
          chains.Add(chain);
        }

        return chains;
      });
    }

    public T ById(string id)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<T>(id);
      });
    }

    public T Create(T review)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          review.Id = UpdateId(review.Id);
          _dbConnection.Value.Insert(review, trans);
          trans.Commit();

          return review;
        }
      });
    }

    public void Delete(T review)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          _dbConnection.Value.Delete(review, trans);
          trans.Commit();

          return 0;
        }
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
