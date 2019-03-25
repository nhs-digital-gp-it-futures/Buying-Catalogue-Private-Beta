using Gif.Service.Attributes;
using Gif.Service.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
  [CrmEntity("cc_standardapplicables")]
  [DataContract]
  public class StandardApplicable : ClaimsBase
  {
    [DataMember]
    [CrmIdField]
    [CrmFieldName("cc_standardapplicableid")]
    public Guid Id { get; set; }

    [DataMember]
    [CrmFieldName("cc_startdateandtime")]
    public DateTime? StartDateTime { get; set; }

    [DataMember]
    [CrmFieldName("cc_enddateandtime")]
    public DateTime? EndDateTime { get; set; }

    [DataMember]
    [CrmFieldName("cc_isdeleted")]
    public bool IsDeleted { get; set; }

    [DataMember]
    [CrmFieldName("cc_name")]
    public string Name { get; set; }

    [DataMember]
    [CrmFieldName("_cc_previousversion_value")]
    [CrmFieldNameDataBind("cc_PreviousVersion@odata.bind")]
    [CrmFieldEntityDataBind("cc_standardapplicables")]
    public Guid? PreviousVersion { get; set; }

    [DataMember]
    [CrmFieldName("_cc_solution_value")]
    [CrmFieldNameDataBind("cc_Solution@odata.bind")]
    [CrmFieldEntityDataBind("cc_solutions")]
    public Guid? SolutionId { get; set; }

    [DataMember]
    [CrmFieldName("_cc_standard_value")]
    [CrmFieldNameDataBind("cc_Standard@odata.bind")]
    [CrmFieldEntityDataBind("cc_standards")]
    public Guid? StandardId { get; set; }

    [DataMember]
    [CrmFieldName("_cc_ownerid_value")]
    [CrmFieldNameDataBind("cc_OwnerId@odata.bind")]
    [CrmFieldEntityDataBind("contacts")]
    public Guid? OwnerId { get; set; }

    [DataMember]
    [CrmFieldName("statuscode")]
    public StandardApplicableStatusEnum Status { get; set; }

    [DataMember]
    [CrmFieldName("cc_submittedon")]
    public DateTime SubmittedOn { get; set; }

    [DataMember]
    [CrmFieldName("cc_assignedon")]
    public DateTime AssignedOn { get; set; }

    public StandardApplicable() { }

    public StandardApplicable(JToken token) : base(token)
    {
    }
  }
}
