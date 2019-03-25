#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Gif.Service.Crm
{
  public interface IRepository
  {
    JToken RetrieveMultiple(string query, out int? count);
    void Associate(Guid entityId1, string entName1, Guid entityId2, string entName2, string relationshipKey);
    Guid CreateEntity(string entityName, string entityData, bool update = false);
    void UpdateEntity(string entityName, Guid id, string entityData);
    void CreateBatch(List<BatchData> batchData);
    void Delete(string entityName, Guid id);
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
