using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public sealed class DummyAuthenticationHandler : IAuthenticationHandler
  {
    public Task<AuthenticateResult> AuthenticateAsync()
    {
      throw new NotImplementedException();
    }

    public Task ChallengeAsync(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties)
    {
      throw new NotImplementedException();
    }

    public Task ForbidAsync(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties)
    {
      throw new NotImplementedException();
    }

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
      throw new NotImplementedException();
    }
  }
}
;