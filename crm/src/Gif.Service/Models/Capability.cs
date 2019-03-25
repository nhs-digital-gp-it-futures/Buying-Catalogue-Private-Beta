using Gif.Service.Attributes;
using Gif.Service.Const;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    [CrmEntity("cc_capabilities")]
    [DataContract]
    public class Capability : EntityBase
    {
        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_capabilityid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("cc_name")]
        public string Name { get; set; }

        [DataMember]
        [CrmFieldName("cc_description")]
        public string Description { get; set; }

        [DataMember]
        [CrmFieldName("cc_url")]
        public string Url { get; set; }

        [DataMember]
        [CrmFieldName("cc_type")]
        public string Type { get; set; }

        [CrmEntityRelationAttribute(RelationshipNames.CapabilityFramework)]
        public IList<Framework> Frameworks { get; set; }

        public Capability() { }

        public Capability(JToken token) : base(token)
        {
        }
    }
}


