#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
    [CrmEntity("accounts")]
    [DataContract]
    public class Organisation : EntityBase
    {
        [CrmFieldName("accountid")]
        [CrmIdField]
        [DataMember]
        public Guid Id { get; set; }

        [CrmFieldName("name")]
        [DataMember]
        public string Name { get; set; }

        [CrmFieldName("description")]
        [DataMember]
        public string Description { get; set; }

        [CrmFieldName("cc_primaryroleid")]
        [DataMember]
        public string PrimaryRoleId { get; set; }

        public Organisation() { }

        public Organisation(JToken token) : base(token)
        {
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member