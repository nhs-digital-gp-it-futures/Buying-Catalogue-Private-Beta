#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Gif.Service.Attributes;
using Gif.Service.Const;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
  public class StandardsApplicableEvidenceService : ServiceBase<StandardApplicableEvidence>, IStandardsApplicableEvidenceDatastore
  {
    public StandardsApplicableEvidenceService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<IEnumerable<StandardApplicableEvidence>> ByClaim(string claimId)
    {
      var evidenceList = new List<StandardApplicableEvidence>();
      var evidenceListList = new List<List<StandardApplicableEvidence>>();

      // get all items at the end of the chain i.e. where the previous id is null
      var filterEvidenceParent = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("ClaimId") {FilterName = "_cc_standardapplicable_value", FilterValue = claimId},
                new CrmFilterAttribute("Previous") {FilterName = "_cc_previousversion_value", FilterValue = "null"},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var jsonEvidenceParent = Repository.RetrieveMultiple(new StandardApplicableEvidence().GetQueryString(null, filterEvidenceParent, true, true), out Count);

      // iterate through all items that are at the end of the chain
      foreach (var evidenceChild in jsonEvidenceParent.Children())
        AddEvidenceChainToList(evidenceChild, evidenceList, evidenceListList);

      Count = evidenceListList.Count;

      return evidenceListList;
    }

    public IEnumerable<IEnumerable<StandardApplicableEvidence>> ByClaimMultiple(List<Guid> claimIds)
    {
        var evidenceListList = new List<List<StandardApplicableEvidence>>();

        // get all items at the end of the chain i.e. where the previous id is null
        var filterEvidenceParent = new List<CrmFilterAttribute>
        {
            new CrmFilterAttribute("Previous") {FilterName = "_cc_previousversion_value", FilterValue = "null"},
            new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
        };

        foreach (var claim in claimIds)
        {
            filterEvidenceParent.Add(new CrmFilterAttribute("ClaimId")
                { FilterName = "_cc_standardapplicable_value", FilterValue = claim.ToString(), MultiConditional = true });
        }

        var jsonEvidenceParent = Repository.RetrieveMultiple(new StandardApplicableEvidence().GetQueryString(null, filterEvidenceParent, true, true), out Count);

        foreach (var evidenceChild in jsonEvidenceParent.Children())
            AddEvidenceChainToList(evidenceChild, new List<StandardApplicableEvidence>(), evidenceListList);

        Count = evidenceListList.Count;

        return evidenceListList;
     }

    private void AddEvidenceChainToList(JToken evidence, List<StandardApplicableEvidence> evidenceList, List<List<StandardApplicableEvidence>> evidenceListList)
    {
      GetEvidencesChain(evidence, evidenceList);

      var enumEvidenceList = StandardApplicableEvidence.OrderLinkedEvidences(evidenceList);
      evidenceListList.Add(enumEvidenceList.ToList());
    }

    private void GetEvidencesChain(JToken evidenceChainEnd, List<StandardApplicableEvidence> evidenceList)
    {
      // store the end of the chain (with no previous id)
      var evidence = new StandardApplicableEvidence(evidenceChainEnd);
      evidenceList.Add(evidence);
      var id = evidence.Id.ToString();

      // get the chain of evidences linked by previous id
      while (true)
      {
        var filterEvidence = new List<CrmFilterAttribute>
        {
            new CrmFilterAttribute("ClaimId") {FilterName = "_cc_standardapplicable_value", FilterValue = id},
            new CrmFilterAttribute("PreviousEvidence") {FilterName = "_cc_previousversion_value", FilterValue = id},
            new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
        };

        var jsonEvidence = Repository.RetrieveMultiple(new StandardApplicableEvidence().GetQueryString(null, filterEvidence, true, true), out Count);
        if (jsonEvidence.HasValues)
        {
          evidence = new StandardApplicableEvidence(jsonEvidence.FirstOrDefault());
          evidenceList.Add(evidence);
          id = evidence.Id.ToString();
        }
        else
          break;
      }
    }

    public StandardApplicableEvidence ById(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("EvidenceId") {FilterName = "cc_evidenceid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new StandardApplicableEvidence().GetQueryString(null, filterAttributes), out Count);
      var evidence = appJson?.FirstOrDefault();

      return (evidence == null) ? null : new StandardApplicableEvidence(evidence);
    }

    public StandardApplicableEvidence Create(StandardApplicableEvidence evidenceEntity)
    {
      Repository.CreateEntity(evidenceEntity.EntityName, evidenceEntity.SerializeToODataPost());

      return evidenceEntity;
    }

    public StandardApplicable ByEvidenceId(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("EvidenceId") {FilterName = "cc_evidenceid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new StandardApplicableEvidence().GetQueryString(null, filterAttributes, true), out Count);
      var standardApplicable = appJson?.Children().FirstOrDefault();

      var standardApplicableRecord = standardApplicable?[RelationshipNames.EvidenceStandardApplicables];

      return standardApplicableRecord != null ?
          new StandardApplicable(standardApplicableRecord) : null;
    }

  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
