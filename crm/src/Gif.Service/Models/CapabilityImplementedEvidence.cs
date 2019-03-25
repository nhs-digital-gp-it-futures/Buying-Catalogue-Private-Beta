#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Gif.Service.Const;

namespace Gif.Service.Models
{
    public class CapabilityImplementedEvidence : EvidenceBase
    {
        [CrmEntityRelationAttribute(RelationshipNames.EvidenceCapabilityImplemented)]
        public IList<CapabilityImplemented> CapabilityImplemented { get; set; }

        public CapabilityImplementedEvidence()
        {
        }

        public CapabilityImplementedEvidence(JToken token) : base(token)
        {
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
