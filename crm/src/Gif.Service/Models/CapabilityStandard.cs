#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Const;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    /// <summary>
    /// Link between a Capability and a Standard
    /// </summary>
    [CrmEntity("cc_capabilitystandards")]
    [DataContract]
    public class CapabilityStandard : EntityBase
    {
        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_capabilitystandardid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("_cc_capability_value")]
        [CrmFieldNameDataBind("cc_Capability@odata.bind")]
        [CrmFieldEntityDataBind("cc_capabilities")]
        public Guid? CapabilityId { get; set; }

        [DataMember]
        [CrmFieldName("_cc_standard_value")]
        [CrmFieldNameDataBind("cc_Standard@odata.bind")]
        [CrmFieldEntityDataBind("cc_standards")]
        public Guid? StandardId { get; set; }

        /// <summary>
        /// True if the Standard does not have to be supported in order to support the Capability
        /// </summary>
        [DataMember]
        [CrmFieldName("cc_isoptional")]
        public bool? IsOptional { get; set; }


        [CrmEntityRelationAttribute(RelationshipNames.CapabilityStandardStandard)]
        public IList<Standard> StandardRelationship { get; set; }


        [CrmEntityRelationAttribute(RelationshipNames.CapabilityStandardCapability)]
        public IList<Capability> CapabilityRelationship { get; set; }

        public CapabilityStandard() { }

        public CapabilityStandard(JToken token) : base(token)
        {
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
