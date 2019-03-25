using System.Runtime.Serialization;

namespace Gif.Service.Enums
{
    public enum CapabilityImplementedStatus
    {
        [EnumMember(Value = "Approved")]
        Approved = 948120005,

        [EnumMember(Value = "Draft")]
        Draft = 948120002,

        [EnumMember(Value = "Rejected")]
        Rejected = 948120006,

        [EnumMember(Value = "Remediation")]
        Remediation = 948120004,

        [EnumMember(Value = "Submitted")]
        Submitted = 948120003
    }
}
