using Gif.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace Gif.Service.Authentications
{
  public sealed class BasicAuthentication : IBasicAuthentication
  {
    private readonly IHostingEnvironment _env;

    public BasicAuthentication(IHostingEnvironment env)
    {
      _env = env;
    }

    public Task Authenticate(ValidatePrincipalContext context)
    {
      if (!_env.IsDevelopment())
      {
        context.AuthenticationFailMessage = "Basic authentication only available in Development environment";

        return Task.CompletedTask;
      }

      // use basic authentication to support Swagger
      if (context.UserName != context.Password)
      {
        context.AuthenticationFailMessage = "Authentication failed.";

        return Task.CompletedTask;
      }

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer),

        // use (case-sensitive) UserName for role
        new Claim(ClaimTypes.Role, context.UserName)
      };

      context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme));

      return Task.CompletedTask;
    }
  }
}
