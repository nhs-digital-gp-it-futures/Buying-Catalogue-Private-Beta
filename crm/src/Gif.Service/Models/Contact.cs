using Gif.Service.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    [CrmEntity("contacts")]
    [DataContract]
    public class Contact : EntityBase
    {
        [DataMember]
        [CrmIdField]
        [CrmFieldName("contactid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("firstname")]
        public string FirstName { get; set; }

        [DataMember]
        [CrmFieldName("lastname")]
        public string LastName { get; set; }

        [DataMember]
        [CrmFieldName("emailaddress1")]
        public string EmailAddress1 { get; set; }

        [DataMember]
        [CrmFieldName("_parentcustomerid_value")]
        [CrmFieldNameDataBind("ParentCustomerId@odata.bind")]
        [CrmFieldEntityDataBind("accounts")]
        public Guid? OrganisationId { get; set; }

        public Contact() { }

        public Contact(JToken token) : base(token)
        {
        }
    }
}
