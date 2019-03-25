#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Const;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    public class StandardApplicableEvidence : EvidenceBase
    {
        [DataMember]
        [CrmFieldName("_cc_standardapplicable_value")]
        [CrmFieldNameDataBind("cc_StandardApplicable@odata.bind")]
        [CrmFieldEntityDataBind("cc_standardapplicables")]
        public override Guid? ClaimId { get; set; }

        [CrmEntityRelationAttribute(RelationshipNames.EvidenceStandardApplicables)]
        public IList<StandardApplicable> StandardApplicables { get; set; }

        public StandardApplicableEvidence()
        {
        }
        public StandardApplicableEvidence(JToken token) : base(token)
        {
        }

        public static IEnumerable<StandardApplicableEvidence> OrderLinkedEvidences(IEnumerable<StandardApplicableEvidence> evidences)
        {
            List<StandardApplicableEvidence> enumEvidences = evidences.ToList();
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
