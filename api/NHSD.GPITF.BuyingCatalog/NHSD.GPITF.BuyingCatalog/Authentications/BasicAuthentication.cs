using Microsoft.AspNetCore.Hosting;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace NHSD.GPITF.BuyingCatalog.Authentications
{
#pragma warning disable CS1591
  public sealed class BasicAuthentication : IBasicAuthentication
  {
    private readonly IContactsDatastore _contactDatastore;
    private readonly IOrganisationsDatastore _organisationDatastore;
    private readonly IHostingEnvironment _env;

    public BasicAuthentication(
      IContactsDatastore contactDatastore,
      IOrganisationsDatastore organisationDatastore,
      IHostingEnvironment env
      )
    {
      _contactDatastore = contactDatastore;
      _organisationDatastore = organisationDatastore;
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

      var primaryRoleId = string.Empty;
      var email = string.Empty;
      switch (context.UserName)
      {
        case Roles.Admin:
        case Roles.Buyer:
          primaryRoleId = PrimaryRole.GovernmentDepartment;
          email = "buying.catalogue.assessment@gmail.com";
          break;

        case Roles.Supplier:
          primaryRoleId = PrimaryRole.ApplicationServiceProvider;
          email = "buying.catalogue.supplier@gmail.com";
          break;

        default:
          break;
      }

      var contact = _contactDatastore.ByEmail(email);
      var org = _organisationDatastore.ByContact(contact?.Id ?? string.Empty);
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Email, email, context.Options.ClaimsIssuer),
        new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer),

        // use (case-sensitive) UserName for role
        new Claim(ClaimTypes.Role, context.UserName),

        // random organisation for Joe public
        new Claim(nameof(Organisations), org?.Id ?? Guid.NewGuid().ToString())
      };

      context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme));

      return Task.CompletedTask;
    }
  }
#pragma warning restore CS1591
}
