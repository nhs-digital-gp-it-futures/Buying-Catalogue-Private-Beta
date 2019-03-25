using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Logic;
using System;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyFilterBase : FilterBase<object>
  {
    public DummyFilterBase(IHttpContextAccessor context) :
      base(context)
    {
    }

    public override object Filter(object input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("Null input should be filtered out in FilterInternal");
      }
      return input;
    }
  }
}
