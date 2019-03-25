using Gif.Service.Attributes;
using Gif.Service.Const;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    [CrmEntity("cc_standards")]
    [DataContract]
    public class Standard : EntityBase
    {
        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_standardid")]
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
        [CrmFieldName("cc_isoverarching")]
        public bool? IsOverarching { get; set; }

        [DataMember]
        [CrmFieldName("cc_previousversion")]
        public string PreviousId { get; set; }

        [DataMember]
        [CrmFieldName("cc_type")]
        public string Type { get; set; }

        [CrmEntityRelationAttribute(RelationshipNames.StandardFramework)]
        public IList<Framework> Frameworks { get; set; }

        public Standard() { }

        public Standard(JToken token) : base(token)
        {
        }
    }
}


