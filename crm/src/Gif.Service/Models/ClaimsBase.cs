#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace Gif.Service.Models
{
  public abstract class ClaimsBase : EntityBase
  {
    [DataMember]
    [CrmFieldName("cc_originaldate")]
    public DateTime OriginalDate { get; set; }

    public ClaimsBase()
    {
    }

    public ClaimsBase(JToken token) :
      base(token)
    {
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
