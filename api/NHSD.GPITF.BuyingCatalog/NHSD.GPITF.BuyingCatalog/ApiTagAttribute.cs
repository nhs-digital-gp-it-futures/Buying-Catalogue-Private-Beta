using System;

namespace NHSD.GPITF.BuyingCatalog
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
  internal sealed class ApiTagAttribute : Attribute
  {
    public string Tag { get; }

    public ApiTagAttribute(string tag)
    {
      Tag = tag;
    }
  }
}
