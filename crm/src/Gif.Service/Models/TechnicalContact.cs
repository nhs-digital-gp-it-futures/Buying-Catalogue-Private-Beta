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
    [CrmEntity("cc_technicalcontacts")]
    [DataContract]
    public class TechnicalContact : EntityBase
    {
        [DataMember]
        [CrmIdField]
        [CrmFieldName("cc_technicalcontactid")]
        public Guid Id { get; set; }

        [DataMember]
        [CrmFieldName("cc_firstname")]
        public string FirstName { get; set; }

        [DataMember]
        [CrmFieldName("cc_lastname")]
        public string LastName { get; set; }

        [DataMember]
        [CrmFieldName("cc_emailaddress")]
        public string EmailAddress { get; set; }

        [DataMember]
        [CrmFieldName("cc_phonenumber")]
        public string PhoneNumber { get; set; }

        [DataMember]
        [CrmFieldName("cc_contacttypeid")]
        public string ContactType { get; set; }

        [DataMember]
        [CrmFieldName("_cc_solution_value")]
        [CrmFieldNameDataBind("cc_Solution@odata.bind")]
        [CrmFieldEntityDataBind("cc_solutions")]
        public Guid SolutionId { get; set; }

        public TechnicalContact() { }

        public TechnicalContact(JToken token) : base(token)
        {
        }
    }
}
