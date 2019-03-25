using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Gif.Service.Enums
{
    public enum BatchTypeEnum
    {
        [EnumMember(Value = "Create")]
        Create,
        [EnumMember(Value = "Read")]
        Read,
        [EnumMember(Value = "Update")]
        Update,
        [EnumMember(Value = "Delete")]
        Delete,
    }
}
