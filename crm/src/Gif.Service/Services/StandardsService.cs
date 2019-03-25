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
  public class StandardsService : ServiceBase<Standard>, IStandardsDatastore
  {
    public StandardsService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<Standard> ByCapability(string capabilityId, bool? isOptional)
    {
      var standards = new List<Standard>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Capability") {FilterName = "_cc_capability_value", FilterValue = capabilityId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      if (isOptional != null)
        filterAttributes.Add(new CrmFilterAttribute("IsOptional")
        { FilterName = "cc_isoptional", FilterValue = isOptional.ToString().ToLower(), QuotesRequired = false });

      var appJson = Repository.RetrieveMultiple(new CapabilityStandard().GetQueryString(null, filterAttributes, true, true), out Count);

      foreach (var item in appJson)
      {
        if (item[RelationshipNames.CapabilityStandardStandard] == null)
          return null;

        var capabilitiesJson = item[RelationshipNames.CapabilityStandardStandard];

        standards.Add(new Standard(capabilitiesJson));
      }

      Count = standards.Count();

      return standards;
    }

    public IEnumerable<Standard> ByFramework(string frameworkId)
    {
      var standards = new List<Standard>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Framework") {FilterName = "cc_frameworkid", FilterValue = frameworkId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Framework().GetQueryString(null, filterAttributes, true, true), out Count);

      var standard = appJson.Children().FirstOrDefault();

      if (standard?[RelationshipNames.StandardFramework] != null)
      {
        foreach (var retrievedStandard in standard[RelationshipNames.StandardFramework].Children())
        {
          standards.Add(new Standard(retrievedStandard));
        }
      }

      Count = standards.Count();

      return standards;
    }

    public Standard ById(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Standard") {FilterName = "cc_standardid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Standard().GetQueryString(null, filterAttributes), out int? count);
      var standard = appJson?.FirstOrDefault();

      return (standard == null) ? null : new Standard(standard);
    }

    public IEnumerable<Standard> ByIds(IEnumerable<string> ids)
    {
      var standardList = new List<Standard>();

      foreach (var id in ids)
      {
        var filterAttributes = new List<CrmFilterAttribute>
                {
                    new CrmFilterAttribute("Standard") {FilterName = "cc_standardid", FilterValue = id},
                    new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
                };

        var appJson = Repository.RetrieveMultiple(new Standard().GetQueryString(null, filterAttributes, true, true), out Count);

        var standard = appJson?.FirstOrDefault();

        if (standard != null)
          standardList.Add(new Standard(standard));
      }

      Count = standardList.Count();

      return standardList;
    }

    public IEnumerable<Standard> GetAll()
    {
      var standardList = new List<Standard>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Statecode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Standard().GetQueryString(null, filterAttributes, false, true), out Count);

      foreach (var standard in appJson.Children())
      {
        standardList.Add(new Standard(standard));
      }

      return standardList;
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
