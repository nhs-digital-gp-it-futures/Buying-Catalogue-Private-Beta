using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Gif.Service.Contracts;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Gif.Service.Models
{
    public class CapabilityEvidence : EvidenceBase
    {
        public CapabilityEvidence()
        {
        }
        public CapabilityEvidence(JToken token) : base(token)
        {
        }

        public static IEnumerable<CapabilityEvidence> OrderLinkedEvidences(IEnumerable<CapabilityEvidence> evidences)
        {
            List<CapabilityEvidence> enumEvidences = evidences.ToList();
            var evidence = enumEvidences.FirstOrDefault(x => x.PreviousId == null);
            int count = enumEvidences.Count();

            if (evidence != null)
            {
                var prevEvidence = evidence;
                prevEvidence.Order = count;

                while (count > 0)
                {
                    count--;
                    prevEvidence = enumEvidences.FirstOrDefault(x => prevEvidence != null && (x.PreviousId != null && x.PreviousId.Value == prevEvidence.Id));
                    if (prevEvidence != null)
                        prevEvidence.Order = count;
                }
            }

            var orderedEvidences = enumEvidences.OrderBy(x => x.Order);
            return orderedEvidences;
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
