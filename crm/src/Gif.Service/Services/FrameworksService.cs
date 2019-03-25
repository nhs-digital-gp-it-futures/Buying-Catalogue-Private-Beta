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
  public class FrameworksService : ServiceBase<Framework>, IFrameworksDatastore
  {
    public FrameworksService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<Framework> ByCapability(string capabilityId)
    {
      var frameworks = new List<Framework>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Capability") {FilterName = "cc_capabilityid", FilterValue = capabilityId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Capability().GetQueryString(null, filterAttributes, true, true), out Count);

      var capability = appJson.Children().FirstOrDefault();

      if (capability?[RelationshipNames.CapabilityFramework] != null)

        foreach (var framework in capability[RelationshipNames.CapabilityFramework].Children())
        {
          frameworks.Add(new Framework(framework));
        }

      Count = frameworks.Count();

      return frameworks;
    }

    public Framework ById(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("FrameworkId") {FilterName = "cc_frameworkid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Framework().GetQueryString(null, filterAttributes), out Count);
      var frameworkJson = appJson?.FirstOrDefault();

      return (frameworkJson == null) ? null : new Framework(frameworkJson);
    }

    public IEnumerable<Framework> BySolution(string solutionId)
    {
      var frameworks = new List<Framework>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Solution") {FilterName = "cc_solutionid", FilterValue = solutionId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Solution().GetQueryString(null, filterAttributes, true, true), out Count);

      var solution = appJson.Children().FirstOrDefault();

      if (solution?[RelationshipNames.SolutionFramework] != null)
      {
        foreach (var framework in solution[RelationshipNames.SolutionFramework].Children())
        {
          frameworks.Add(new Framework(framework));
        }
      }

      Count = frameworks.Count();

      return frameworks;
    }

    public IEnumerable<Framework> ByStandard(string standardId)
    {
      var frameworks = new List<Framework>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Standard") {FilterName = "cc_standardid", FilterValue = standardId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Standard().GetQueryString(null, filterAttributes, true, true), out Count);

      var standard = appJson.Children().FirstOrDefault();

      if (standard?[RelationshipNames.StandardFramework] != null)
      {
        foreach (var framework in standard[RelationshipNames.StandardFramework].Children())
        {
          frameworks.Add(new Framework(framework));
        }
      }

      Count = frameworks.Count();

      return frameworks;
    }

    public IEnumerable<Framework> GetAll()
    {
      var frameworks = new List<Framework>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Statecode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Framework().GetQueryString(null, filterAttributes, false, true), out Count);

      foreach (var framework in appJson.Children())
      {
        frameworks.Add(new Framework(framework));
      }

      return frameworks;
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
