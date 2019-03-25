using Gif.Service.Attributes;
using Gif.Service.Const;
using Gif.Service.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    [CrmEntity("cc_solutions")]
    [DataContract]
    public class Solution : EntityBase
    {
        private string _productPage = string.Empty;
        private string _version = string.Empty;
        private string _description = string.Empty;

        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_solutionid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("cc_name")]
        public string Name { get; set; }

        [DataMember]
        [CrmFieldName("cc_description")]
        public string Description
        {
            get => _description ?? string.Empty;
            set => _description = value;
        }

        [DataMember]
        [CrmFieldName("statuscode")]
        public SolutionStatusEnum? Status { get; set; }

        [DataMember]
        [CrmFieldName("_cc_previous_version_value")]
        [CrmFieldNameDataBind("cc_Previous_Version@odata.bind")]
        [CrmFieldEntityDataBind("cc_solutions")]
        public Guid? PreviousId { get; set; }

        [DataMember]
        [CrmFieldName("_cc_organisationid_value")]
        [CrmFieldNameDataBind("cc_OrganisationId@odata.bind")]
        [CrmFieldEntityDataBind("accounts")]
        public Guid? OrganisationId { get; set; }

        [DataMember]
        [CrmFieldName("cc_version")]
        public string Version
        {
            get => _version ?? string.Empty;
            set => _version = value;
        }

        [DataMember]
        [CrmFieldName("_cc_createdbyid_value")]
        [CrmFieldNameDataBind("cc_CreatedByID@odata.bind")]
        [CrmFieldEntityDataBind("contacts")]
        public Guid? CreatedById { get; set; }

        [DataMember]
        [CrmFieldName("_cc_modifiedby_value")]
        [CrmFieldNameDataBind("cc_ModifiedBy@odata.bind")]
        [CrmFieldEntityDataBind("contacts")]
        public Guid? ModifiedById { get; set; }

        [DataMember]
        [CrmFieldName("cc_productpage")]
        public string ProductPage
        {
            get => _productPage ?? string.Empty;
            set => _productPage = value;
        }

        [CrmEntityRelation(RelationshipNames.SolutionFramework)]
        public IList<Framework> Frameworks { get; set; }

        public Solution() { }

        public Solution(JToken token) : base(token)
        {
        }
    }
}
