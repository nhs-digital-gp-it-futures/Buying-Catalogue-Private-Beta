using Gif.Plugins.Contracts;
using Gif.Plugins.PluginEntities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Plugins.Repositories
{
    public class SolutionRepository : Repository, ISolutionRepository
    {
        public SolutionRepository(IOrganizationService svc) : base(svc)
        {
        }

        public IEnumerable<cc_technicalcontact> GetTechnicalContactsBySolution(Guid id)
        {
            List<cc_technicalcontact> technicalContacts = null;

            using (var ctx = new OrganizationServiceContext(Svc))
            {
                technicalContacts = ctx.CreateQuery<cc_technicalcontact>()
                    .Where(x => x.cc_Solution.Id == id &&
                                x.StateCode == cc_technicalcontactState.Active)
                    .Select(x => new cc_technicalcontact
                    {
                        Id = x.Id
                    })
                    .ToList();
            }

            return technicalContacts;
        }

        public IEnumerable<cc_capabilityimplemented> GetCapabilitiesImplementedBySolution(Guid id)
        {
            List<cc_capabilityimplemented> capabilitiesImplemented = null;

            using (var ctx = new OrganizationServiceContext(Svc))
            {
                capabilitiesImplemented = ctx.CreateQuery<cc_capabilityimplemented>()
                    .Where(x => x.cc_Solution.Id == id &&
                                x.StateCode == cc_capabilityimplementedState.Active)
                    .Select(x => new cc_capabilityimplemented
                    {
                        Id = x.Id
                    })
                    .ToList();
            }

            return capabilitiesImplemented;
        }

        public IEnumerable<cc_standardapplicable> GetStandardsApplicableBySolution(Guid id)
        {
            List<cc_standardapplicable> standardsApplicable = null;

            using (var ctx = new OrganizationServiceContext(Svc))
            {
                standardsApplicable = ctx.CreateQuery<cc_standardapplicable>()
                    .Where(x => x.cc_Solution.Id == id &&
                                x.StateCode == cc_standardapplicableState.Active)
                    .Select(x => new cc_standardapplicable
                    {
                        Id = x.Id
                    })
                    .ToList();
            }

            return standardsApplicable;
        }

        public IEnumerable<cc_evidence> GetEvidencesByCapabilityImplemented(Guid id)
        {
            List<cc_evidence> evidences = null;

            using (var ctx = new OrganizationServiceContext(Svc))
            {
                evidences = ctx.CreateQuery<cc_evidence>()
                    .Where(x => x.cc_CapabilityImplemented.Id == id &&
                                x.StateCode == cc_evidenceState.Active)
                    .Select(x => new cc_evidence
                    {
                        Id = x.Id
                    })
                    .ToList();
            }

            return evidences;
        }

        public IEnumerable<cc_evidence> GetEvidencesByStandardApplicable(Guid id)
        {
            List<cc_evidence> evidences = null;

            using (var ctx = new OrganizationServiceContext(Svc))
            {
                evidences = ctx.CreateQuery<cc_evidence>()
                    .Where(x => x.cc_StandardApplicable.Id == id &&
                                x.StateCode == cc_evidenceState.Active)
                    .Select(x => new cc_evidence
                    {
                        Id = x.Id
                    })
                    .ToList();
            }

            return evidences;
        }

        public IEnumerable<cc_review> GetReviewsByEvidence(Guid id)
        {
            List<cc_review> reviews = null;

            using (var ctx = new OrganizationServiceContext(Svc))
            {
                reviews = ctx.CreateQuery<cc_review>()
                    .Where(x => x.cc_Evidence.Id == id &&
                                x.StateCode == cc_reviewState.Active)
                    .Select(x => new cc_review
                    {
                        Id = x.Id
                    })
                    .ToList();
            }

            return reviews;
        }
    }
}
