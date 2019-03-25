#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Const;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
  public class SolutionsService : ServiceBase<Solution>, ISolutionsDatastore
  {
    public SolutionsService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<Solution> ByFramework(string frameworkId)
    {
      var solutions = new List<Solution>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Framework") {FilterName = "cc_frameworkid", FilterValue = frameworkId}
            };

      var appJson = Repository.RetrieveMultiple(new Framework().GetQueryString(null, filterAttributes, true, true), out Count);

      var framework = appJson.Children().FirstOrDefault();

      if (framework?[RelationshipNames.SolutionFramework] != null)
      {
        foreach (var solution in framework[RelationshipNames.SolutionFramework].Children())
        {
          solutions.Add(new Solution(solution));
        }
      }

      Count = solutions.Count();

      return solutions;
    }

    public Solution ById(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("SolutionId") {FilterName = "cc_solutionid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Solution().GetQueryString(null, filterAttributes), out int? count);
      var solutionJson = appJson?.FirstOrDefault();

      return (solutionJson == null) ? null : new Solution(solutionJson);
    }

    public IEnumerable<Solution> ByOrganisation(string organisationId)
    {
      var solutions = new List<Solution>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Organisation") {FilterName = "_cc_organisationid_value", FilterValue = organisationId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Solution().GetQueryString(null, filterAttributes, true, true), out Count);

      foreach (var solution in appJson.Children())
      {
        solutions.Add(new Solution(solution));
      }

      Count = solutions.Count();

      return solutions;
    }

    public Solution Create(Solution solution)
    {
      Repository.CreateEntity(solution.EntityName, solution.SerializeToODataPost());

      return solution;
    }

    public void Update(Solution solution)
    {
      Repository.UpdateEntity(solution.EntityName, solution.Id, solution.SerializeToODataPut("cc_solutionid"));
    }

    public void Delete(Solution solution)
    {
      Repository.Delete(solution.EntityName, solution.Id);
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
