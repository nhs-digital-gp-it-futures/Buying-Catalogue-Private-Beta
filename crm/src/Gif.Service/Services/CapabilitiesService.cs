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
  public class CapabilitiesService : ServiceBase<Capability>, ICapabilityDatastore
  {
    public CapabilitiesService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<Capability> ByFramework(string frameworkId)
    {
      var capabilities = new List<Capability>();

      var filterAttributes = new List<CrmFilterAttribute>
                {
                    new CrmFilterAttribute("Framework") {FilterName = "cc_frameworkid", FilterValue = frameworkId},
                    new CrmFilterAttribute("Statecode") {FilterName = "statecode", FilterValue = "0"}
                };


      var appJson = Repository.RetrieveMultiple(new Framework().GetQueryString(null, filterAttributes, true, true), out Count);

      var framework = appJson.Children().FirstOrDefault();

      if (framework?[RelationshipNames.CapabilityFramework] != null)
      {
        foreach (var capability in framework[RelationshipNames.CapabilityFramework].Children())
        {
          capabilities.Add(new Capability(capability));
        }
      }

      Count = capabilities.Count();

      return capabilities;
    }

    public Capability ById(string id)
    {
      Capability capability = null;

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("CapabilityId") {FilterName = "cc_capabilityid", FilterValue = id},
                new CrmFilterAttribute("Statecode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new Capability().GetQueryString(null, filterAttributes), out Count);
      var capabilityJson = appJson?.FirstOrDefault();

      return (capabilityJson == null) ? null : new Capability(capabilityJson);
    }

    public IEnumerable<Capability> ByIds(IEnumerable<string> ids)
    {
      var capabilityList = new List<Capability>();

      foreach (var id in ids)
      {
        var filterAttributes = new List<CrmFilterAttribute>
                {
                    new CrmFilterAttribute("Capability") {FilterName = "cc_capabilityid", FilterValue = id},
                    new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
                };

        var appJson = Repository.RetrieveMultiple(new Capability().GetQueryString(null, filterAttributes, false, true), out Count);

        var capability = appJson?.FirstOrDefault();

        if (capability != null)
          capabilityList.Add(new Capability(capability));
      }

      Count = capabilityList.Count();

      return capabilityList;
    }

    public IEnumerable<Capability> ByStandard(string standardId, bool isOptional)
    {
      var capabilities = new List<Capability>();


      var filterAttributes = new List<CrmFilterAttribute>
                {
                    new CrmFilterAttribute("Standard") {FilterName = "_cc_standard_value", FilterValue = standardId},
                    new CrmFilterAttribute("Statecode") {FilterName = "statecode", FilterValue = "0"},
                };

      var appJson = Repository.RetrieveMultiple(new CapabilityStandard().GetQueryString(null, filterAttributes, true, true), out Count);

      foreach (var item in appJson)
      {
        if (item[RelationshipNames.CapabilityStandardCapability] == null)
          return null;

        var capabilitiesJson = item[RelationshipNames.CapabilityStandardCapability];

        capabilities.Add(new Capability(capabilitiesJson));
      }

      Count = capabilities.Count();

      return capabilities;
    }

    public IEnumerable<Capability> GetAll()
    {
      var capabilities = new List<Capability>();

      var filterAttributes = new List<CrmFilterAttribute>
                {
                    new CrmFilterAttribute("Statecode") {FilterName = "statecode", FilterValue = "0"}
                };

      var appJson = Repository.RetrieveMultiple(new Capability().GetQueryString(null, filterAttributes, false, true), out Count);

      foreach (var capability in appJson.Children())
      {
        capabilities.Add(new Capability(capability));
      }

      Count = capabilities.Count();

      return capabilities;
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
