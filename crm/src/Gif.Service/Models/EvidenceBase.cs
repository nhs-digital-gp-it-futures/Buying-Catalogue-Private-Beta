#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    [CrmEntity("cc_evidences")]
    [DataContract]
    public class EvidenceBase : EntityBase, IHasPreviousId
    {
        private string _evidence = string.Empty;

        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_evidenceid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("cc_name")]
        public string Evidence { get => _evidence ?? string.Empty; set => _evidence = value; }

        [DataMember]
        [CrmFieldName("_cc_capabilityimplemented_value")]
        [CrmFieldNameDataBind("cc_CapabilityImplemented@odata.bind")]
        [CrmFieldEntityDataBind("cc_capabilityimplementeds")]
        public virtual Guid? ClaimId { get; set; }

        [DataMember]
        [CrmFieldName("_cc_createdbyid_value")]
        [CrmFieldNameDataBind("cc_CreatedByID@odata.bind")]
        [CrmFieldEntityDataBind("contacts")]
        public Guid CreatedById { get; set; }

        [DataMember]
        [CrmFieldName("_cc_previousversion_value")]
        [CrmFieldNameDataBind("cc_PreviousVersion@odata.bind")]
        [CrmFieldEntityDataBind("cc_evidences")]
        public Guid? PreviousId { get; set; }

        public int Order { get; set; }

        [DataMember]
        [CrmFieldName("cc_hasrequestedlivedemo")]
        public bool HasRequestedLiveDemo { get; set; }

        [DataMember]
        [CrmFieldName("cc_blobid")]
        public string BlobId { get; set; }

        [DataMember]
        [CrmFieldName("createdon")]
        public new DateTime CreatedOn { get; set; }

        [DataMember]
        [CrmFieldName("modifiedon")]
        public new DateTime ModifiedOn { get; set; }

        [DataMember]
        [CrmFieldName("cc_originaldate")]
        public DateTime OriginalDate { get; set; }

        public EvidenceBase() { }

        public EvidenceBase(JToken token) : base(token)
        {
        }

    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
