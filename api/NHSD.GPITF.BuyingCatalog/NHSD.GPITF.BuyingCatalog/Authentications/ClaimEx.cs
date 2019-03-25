using System.Security.Claims;

namespace NHSD.GPITF.BuyingCatalog.Authentications
{
#pragma warning disable CS1591
  public sealed class ClaimEx
  {
    public ClaimEx()
    {
    }

    public ClaimEx(Claim other)
    {
      Type = other.Type;
      Value = other.Value;
    }

    public string Type { get; set; }
    public string Value { get; set; }
  }
#pragma warning restore CS1591
}

