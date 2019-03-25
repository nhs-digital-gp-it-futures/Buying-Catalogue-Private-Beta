using System.Runtime.Serialization;

namespace Gif.Service.Enums
{
    public enum StandardApplicableStatusEnum
    {
        [EnumMember(Value = "Approved")]
        Approved = 948120004,

        [EnumMember(Value = "Approved First Of Type")]
        ApprovedFirstOfType = 948120005,

        [EnumMember(Value = "ApprovedPartial")]
        ApprovedPartial = 948120006,

        [EnumMember(Value = "Draft")]
        Draft = 948120001,

        [EnumMember(Value = "NotStarted")]
        NotStarted = 948120000,

        [EnumMember(Value = "Rejected")]
        Rejected = 948120007,

        [EnumMember(Value = "Remediation")]
        Remediation = 948120003,

        [EnumMember(Value = "Submitted")]
        Submitted = 948120002
    }

}
