#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
  public class CapabilitiesImplementedService : ServiceBase<CapabilityImplemented>, ICapabilitiesImplementedDatastore
  {
    public CapabilitiesImplementedService(IRepository repository) : base(repository)
    {
    }

    public CapabilityImplemented ById(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("CapabilityImplementedId") {FilterName = "cc_capabilityimplementedid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new CapabilityImplemented().GetQueryString(null, filterAttributes), out Count);
      var capabilityImplementedJson = appJson?.FirstOrDefault();

      return (capabilityImplementedJson == null) ? null : new CapabilityImplemented(capabilityImplementedJson);
    }

    public IEnumerable<CapabilityImplemented> BySolution(string solutionId)
    {
      var capabilitiesImplemented = new List<CapabilityImplemented>();

      var filterAttributes = new List<CrmFilterAttribute>
                {
                    new CrmFilterAttribute("Solution") {FilterName = "_cc_solution_value", FilterValue = solutionId},
                    new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
                };

      var appJson = Repository.RetrieveMultiple(new CapabilityImplemented().GetQueryString(null, filterAttributes, true, true), out Count);

      foreach (var capabilityImplemented in appJson.Children())
      {
        capabilitiesImplemented.Add(new CapabilityImplemented(capabilityImplemented));
      }

      Count = capabilitiesImplemented.Count();

      return capabilitiesImplemented;
    }

    public CapabilityImplemented Create(CapabilityImplemented capabilityImplemented)
    {
      Repository.CreateEntity(capabilityImplemented.EntityName, capabilityImplemented.SerializeToODataPost());

      return capabilityImplemented;
    }

    public void Delete(CapabilityImplemented capabilityImplemented)
    {

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("CapabilityImplemented") {FilterName = "_cc_capabilityimplemented_value", FilterValue = capabilityImplemented.Id.ToString()},
            };

      var capabilityEvidence = new CapabilityEvidence();

      var jsonEvidenceParent = Repository.RetrieveMultiple(capabilityEvidence.GetQueryString(null, filterAttributes), out Count);

      Repository.Delete(capabilityImplemented.EntityName, capabilityImplemented.Id);

      foreach (var evidence in jsonEvidenceParent)
      {
        Repository.Delete(capabilityEvidence.EntityName, new CapabilityEvidence(evidence).Id);
      }
    }

    public void Update(CapabilityImplemented capabilityImplemented)
    {
      Repository.UpdateEntity(capabilityImplemented.EntityName, capabilityImplemented.Id, capabilityImplemented.SerializeToODataPut("cc_capabilityimplementedid"));
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
