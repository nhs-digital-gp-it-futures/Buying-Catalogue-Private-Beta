using Gif.Plugins.PluginEntities;
using System;
using System.Collections.Generic;

namespace Gif.Plugins.Contracts
{
    public interface ISolutionRepository : IRepository
    {
        IEnumerable<cc_technicalcontact> GetTechnicalContactsBySolution(Guid id);
        IEnumerable<cc_capabilityimplemented> GetCapabilitiesImplementedBySolution(Guid id);
        IEnumerable<cc_standardapplicable> GetStandardsApplicableBySolution(Guid id);
        IEnumerable<cc_evidence> GetEvidencesByCapabilityImplemented(Guid id);
        IEnumerable<cc_evidence> GetEvidencesByStandardApplicable(Guid id);
        IEnumerable<cc_review> GetReviewsByEvidence(Guid id);

    }
}
