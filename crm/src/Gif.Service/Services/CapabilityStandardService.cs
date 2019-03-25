#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
  public class CapabilityStandardService : ServiceBase<CapabilityStandard>, ICapabilityStandardDatastore
  {
    public CapabilityStandardService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<CapabilityStandard> GetAll()
    {
      var capabilitiesStandard = new List<CapabilityStandard>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new CapabilityStandard().GetQueryString(null, filterAttributes, true, true), out Count);

      foreach (var capabilityStandard in appJson.Children())
      {
        capabilitiesStandard.Add(new CapabilityStandard(capabilityStandard));
      }

      Count = capabilitiesStandard.Count();

      return capabilitiesStandard;
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
