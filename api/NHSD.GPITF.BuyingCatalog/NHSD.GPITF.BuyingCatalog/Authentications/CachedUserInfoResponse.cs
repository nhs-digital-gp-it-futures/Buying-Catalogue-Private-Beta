using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Authentications
{
#pragma warning disable CS1591
  public sealed class CachedUserInfoResponse
  {
    public IEnumerable<ClaimEx> Claims { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    // required for Json constructor
    public CachedUserInfoResponse()
    {
    }

    public CachedUserInfoResponse(UserInfoResponse userInfoResponse, DateTime created) :
      this(userInfoResponse)
    {
      Created = created;
    }

    public CachedUserInfoResponse(UserInfoResponse userInfoResponse)
    {
      Claims = userInfoResponse.Claims.Select(x => new ClaimEx(x));
    }
  }
#pragma warning restore CS1591
}

