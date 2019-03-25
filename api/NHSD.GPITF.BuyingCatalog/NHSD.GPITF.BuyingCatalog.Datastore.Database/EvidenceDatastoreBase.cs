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
  public abstract class EvidenceDatastoreBase<T> : CommonTableExpressionDatastoreBase<T>, IEvidenceDatastore<EvidenceBase> where T : EvidenceBase
  {
    public EvidenceDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<EvidenceDatastoreBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public T ById(string id)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<T>(id);
      });
    }

    public IEnumerable<IEnumerable<T>> ByClaim(string claimId)
    {
      return GetInternal(() =>
      {
        var table = typeof(T).GetCustomAttribute<TableAttribute>(true);
        var chains = new List<IEnumerable<T>>();
        var sqlAllCurrent = $@"
-- select all current versions
select ev.* from {table.Name} ev where Id not in 
(
  select PreviousId from {table.Name} where PreviousId is not null
) and
ClaimId = @claimId
";
        var allCurrent = _dbConnection.Value.Query<T>(sqlAllCurrent, new { claimId });
        foreach (var current in allCurrent)
        {
          var sqlCurrent = $@"
-- get all previous versions from a specified (CurrentId) version
with recursive Links(CurrentId, Id, PreviousId, ClaimId, CreatedById, CreatedOn, OriginalDate, Evidence, HasRequestedLiveDemo, BlobId) as (
  select
    Id, Id, PreviousId, ClaimId, CreatedById, CreatedOn, OriginalDate, Evidence, HasRequestedLiveDemo, BlobId
  from {table.Name}
  where PreviousId is null
  
  union all
  select
    Id, Id, PreviousId, ClaimId, CreatedById, CreatedOn, OriginalDate, Evidence, HasRequestedLiveDemo, BlobId
  from {table.Name} 
  where PreviousId is not null
  
  union all
  select
    Links.CurrentId,
    {table.Name}.Id,
    {table.Name}.PreviousId,
    {table.Name}.ClaimId,
    {table.Name}.CreatedById,
    {table.Name}.CreatedOn,
    {table.Name}.OriginalDate,
    {table.Name}.Evidence,
    {table.Name}.HasRequestedLiveDemo,
    {table.Name}.BlobId
  from Links
  join {table.Name}
  on Links.PreviousId = {table.Name}.Id
)
  select Links.Id, Links.PreviousId, Links.ClaimId, Links.CreatedById, Links.CreatedOn, Links.OriginalDate, Links.Evidence, Links.HasRequestedLiveDemo, Links.BlobId
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

    public T Create(T evidence)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          evidence.Id = UpdateId(evidence.Id);
          _dbConnection.Value.Insert(evidence, trans);
          trans.Commit();

          return evidence;
        }
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
