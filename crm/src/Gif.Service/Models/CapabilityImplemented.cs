using Gif.Service.Attributes;
using Gif.Service.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    [CrmEntity("cc_capabilityimplementeds")]
    [DataContract]
    public class CapabilityImplemented : ClaimsBase
  {
        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_capabilityimplementedid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("_cc_solution_value")]
        [CrmFieldNameDataBind("cc_Solution@odata.bind")]    // this needs to be Schema name (not logical name)
        [CrmFieldEntityDataBind("cc_solutions")]
        public Guid? SolutionId { get; set; }

        [DataMember]
        [CrmFieldName("_cc_capability_value")]
        [CrmFieldNameDataBind("cc_Capability@odata.bind")]  // this needs to be Schema name (not logical name)
        [CrmFieldEntityDataBind("cc_capabilities")]
        public Guid? CapabilityId { get; set; }

        [DataMember]
        [CrmFieldName("_cc_ownerid_value")]
        [CrmFieldNameDataBind("cc_OwnerId@odata.bind")]
        [CrmFieldEntityDataBind("contacts")]
        public Guid? OwnerId { get; set; }

        [DataMember]
        [CrmFieldName("statuscode")]
        public CapabilityImplementedStatus? Status { get; set; }

        /*[DataMember]
        [CrmFieldName("_modifiedby_value")]
        [CrmFieldNameDataBind("ModifiedBy@odata.bind")]
        [CrmFieldEntityDataBind("contacts")]
        public Guid ModifiedById { get; set; } //Todo*/

        public CapabilityImplemented() { }

        public CapabilityImplemented(JToken token) : base(token)
        {
        }
    }
}
