#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
    public class CapabilitiesImplementedEvidenceService : ServiceBase<CapabilityEvidence>, ICapabilitiesImplementedEvidenceDatastore
    {
        public CapabilitiesImplementedEvidenceService(IRepository repository) : base(repository)
        {
        }

        public IEnumerable<IEnumerable<CapabilityEvidence>> ByClaimMultiple(List<Guid> claimIds)
        {
            var evidenceListList = new List<List<CapabilityEvidence>>();

            // get all items at the end of the chain i.e. where the previous id is null
            var filterEvidenceParent = new List<CrmFilterAttribute>
        {
            new CrmFilterAttribute("Previous") {FilterName = "_cc_previousversion_value", FilterValue = "null"},
            new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
        };

            foreach (var claim in claimIds)
            {
                filterEvidenceParent.Add(new CrmFilterAttribute("ClaimId")
                { FilterName = "_cc_capabilityimplemented_value", FilterValue = claim.ToString(), MultiConditional = true });
            }

            var jsonEvidenceParent = Repository.RetrieveMultiple(new CapabilityEvidence().GetQueryString(null, filterEvidenceParent, true, true), out Count);

            foreach (var evidenceChild in jsonEvidenceParent.Children())
                AddEvidenceChainToList(evidenceChild, new List<CapabilityEvidence>(), evidenceListList);

            Count = evidenceListList.Count;

            return evidenceListList;
        }

        public IEnumerable<IEnumerable<CapabilityEvidence>> ByClaim(string claimId)
        {
            var evidenceListList = new List<List<CapabilityEvidence>>();

            // get all items at the end of the chain i.e. where the previous id is null
            var filterEvidenceParent = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("ClaimId") {FilterName = "_cc_capabilityimplemented_value", FilterValue = claimId},
                new CrmFilterAttribute("Previous") {FilterName = "_cc_previousversion_value", FilterValue = "null"},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

            var jsonEvidenceParent = Repository.RetrieveMultiple(new CapabilityEvidence().GetQueryString(null, filterEvidenceParent, true, true), out Count);

            // iterate through all items that are at the end of the chain
            foreach (var evidenceChild in jsonEvidenceParent.Children())
                AddEvidenceChainToList(evidenceChild, new List<CapabilityEvidence>(), evidenceListList);

            Count = evidenceListList.Count;

            return evidenceListList;
        }

        private void AddEvidenceChainToList(JToken evidence, List<CapabilityEvidence> evidenceList, List<List<CapabilityEvidence>> evidenceListList)
        {
            GetEvidencesChain(evidence, evidenceList);

            var enumEvidenceList = CapabilityEvidence.OrderLinkedEvidences(evidenceList);
            evidenceListList.Add(enumEvidenceList.ToList());
        }

        private void GetEvidencesChain(JToken evidenceChainEnd, List<CapabilityEvidence> evidenceList)
        {
            // store the end of the chain (with no previous id)
            var evidence = new CapabilityEvidence(evidenceChainEnd);
            evidenceList.Add(evidence);
            var id = evidence.Id.ToString();

            // get the chain of evidences linked by previous id
            while (true)
            {
                var filterEvidence = new List<CrmFilterAttribute>
                {
                    new CrmFilterAttribute("PreviousEvidence") {FilterName = "_cc_previousversion_value", FilterValue = id},
                    new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
                };

                var jsonEvidence = Repository.RetrieveMultiple(new CapabilityEvidence().GetQueryString(null, filterEvidence, true, true), out Count);
                if (jsonEvidence.HasValues)
                {
                    evidence = new CapabilityEvidence(jsonEvidence.FirstOrDefault());
                    evidenceList.Add(evidence);
                    id = evidence.Id.ToString();
                }
                else
                    break;
            }
        }

        public CapabilityEvidence ById(string id)
        {
            var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("EvidenceId") {FilterName = "cc_evidenceid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

            var appJson = Repository.RetrieveMultiple(new CapabilityEvidence().GetQueryString(null, filterAttributes), out Count);
            var evidence = appJson?.FirstOrDefault();

            return (evidence == null) ? null : new CapabilityEvidence(evidence);
        }

        public CapabilityEvidence Create(CapabilityEvidence evidenceEntity)
        {
            Repository.CreateEntity(evidenceEntity.EntityName, evidenceEntity.SerializeToODataPost());

            return evidenceEntity;
        }

        public CapabilityImplemented ByEvidenceId(string id)
        {
            var evidence = ById(id);

            if (evidence == null)
                return null;

            var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("CapabilityImplementedId") {FilterName = "cc_capabilityimplementedid", FilterValue = evidence.ClaimId.ToString()},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

            var appJson = Repository.RetrieveMultiple(new CapabilityImplemented().GetQueryString(null, filterAttributes), out Count);
            var capabilityImplemented = appJson?.FirstOrDefault();

            return (capabilityImplemented == null) ? null : new CapabilityImplemented(capabilityImplemented);
        }

        public CapabilityImplemented ByReviewId(string id)
        {
            CapabilityImplemented capabilityImplemented = null;

            var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("ReviewId") {FilterName = "cc_reviewid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

            var reviewJson = Repository.RetrieveMultiple(new Review().GetQueryString(null, filterAttributes), out Count);
            var review = reviewJson?.FirstOrDefault();

            if (review != null)
            {
                var reviewObj = new Review(review);

                if (reviewObj.EvidenceId != null)
                    capabilityImplemented = ByEvidenceId(new Review(review).EvidenceId.ToString());
            }

            return capabilityImplemented;
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
