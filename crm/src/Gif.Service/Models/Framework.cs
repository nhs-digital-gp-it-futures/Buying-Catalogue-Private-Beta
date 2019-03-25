using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gif.Service.Attributes;
using Gif.Service.Const;
using Newtonsoft.Json.Linq;

namespace Gif.Service.Models
{
    [CrmEntity("cc_frameworks")]
    [DataContract]
    public class Framework : EntityBase
    {
        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_frameworkid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("_cc_previousframeworkversion_value")]
        public Guid? PreviousId { get; set; }

        [DataMember]
        [CrmFieldName("cc_name")]
        public string Name { get; set; }

        [DataMember]
        [CrmFieldName("cc_description")]
        public string Description { get; set; }

        [CrmEntityRelationAttribute(RelationshipNames.CapabilityFramework)]
        public IList<Capability> Capabilities { get; set; }

        [CrmEntityRelationAttribute(RelationshipNames.StandardFramework)]
        public IList<Standard> Standards { get; set; }

        [CrmEntityRelationAttribute(RelationshipNames.SolutionFramework)]
        public IList<Solution> Solutions { get; set; }

        public Framework() { }

        public Framework(JToken token) : base(token)
        {
        }
    }
}
