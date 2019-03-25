using System.Runtime.Serialization;

namespace Gif.Service.Enums
{
    public enum SolutionStatusEnum
    {
        [EnumMember(Value = "Approved")]
        Approved = 948120011,

        [EnumMember(Value = "CapabilitiesAssessment")]
        CapabilitiesAssessment = 948120007,

        [EnumMember(Value = "Draft")]
        Draft = 948120005,

        [EnumMember(Value = "Failed")]
        Failed = 948120004,

        [EnumMember(Value = "FinalApproval")]
        FinalApproval = 948120009,

        [EnumMember(Value = "Registered")]
        Registered = 948120006,

        [EnumMember(Value = "SolutionPage")]
        SolutionPage = 948120010,

        [EnumMember(Value = "StandardsCompliance")]
        StandardsCompliance = 948120008,
    }
}
