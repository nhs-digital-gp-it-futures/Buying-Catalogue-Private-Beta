using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NHSD.GPITF.BuyingCatalog.Authentications;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
#pragma warning disable AsyncFixer01
  [TestFixture]
  public sealed class BearerAuthentication_Tests
  {
    private const string BearerToken = "abcdefghijklmno";
    private const string UserInfoEndpoint = "https://localhost/userinfo";
    private const string EmailAddress = "some_person@nhsd.co.uk";

    private TokenValidatedContext _context;
    private Mock<IConfiguration> _config;
    private Mock<ILogger<BearerAuthentication>> _logger;
    private Mock<IUserInfoResponseCache> _cache;
    private Mock<IUserInfoResponseRetriever> _rover;
    private Mock<IContactsDatastore> _contactsDatastore;
    private Mock<IOrganisationsDatastore> _organisationDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = Creator.GetTokenValidatedContext(BearerToken);
      _config = new Mock<IConfiguration>();
      _logger = new Mock<ILogger<BearerAuthentication>>();
      _cache = new Mock<IUserInfoResponseCache>();
      _rover = new Mock<IUserInfoResponseRetriever>();
      _contactsDatastore = new Mock<IContactsDatastore>();
      _organisationDatastore = new Mock<IOrganisationsDatastore>();
    }

    [TestCase(PrimaryRole.ApplicationServiceProvider, Roles.Supplier)]
    [TestCase(PrimaryRole.GovernmentDepartment, Roles.Admin)]
    [TestCase(PrimaryRole.GovernmentDepartment, Roles.Buyer)]
    public async Task Authenticate_OrganisationPrimaryRole_ClaimsRole(string orgPrimaryRole, string expDomainRole)
    {
      var contact = Creator.GetContact();
      var organisation = Creator.GetOrganisation(primaryRoleId: orgPrimaryRole);
      var resp = Creator.GetUserInfoResponse(new[] { ("email", EmailAddress) });
      var expiredResp = Creator.GetCachedUserInfoResponseExpired(resp);
      var expiredRespJson = JsonConvert.SerializeObject(expiredResp);

      _config.Setup(x => x["Jwt:UserInfo"]).Returns(UserInfoEndpoint);
      _cache.Setup(x => x.TryGetValue(It.Is<string>(token => token == BearerToken), out expiredRespJson)).Returns(true);
      _rover.Setup(x => x.GetAsync(UserInfoEndpoint, BearerToken.Substring(7))).ReturnsAsync(resp);
      _contactsDatastore.Setup(x => x.ByEmail(EmailAddress)).Returns(contact);
      _organisationDatastore.Setup(x => x.ByContact(contact.Id)).Returns(organisation);

      var auth = new BearerAuthentication(
        _cache.Object,
        _config.Object,
        _logger.Object,
        _rover.Object,
        _contactsDatastore.Object,
        _organisationDatastore.Object);
      await auth.Authenticate(_context);


      _context.Principal.Claims
        .Should()
        .ContainSingle(x =>
          x.Type == ClaimTypes.Role &&
          x.Value == expDomainRole);
    }

    [Test]
    public async Task Authenticate_OrganisationPrimaryRoleOther_NoRole()
    {
      var contact = Creator.GetContact();
      var organisation = Creator.GetOrganisation(primaryRoleId: "other role");
      var resp = Creator.GetUserInfoResponse(new[] { ("email", EmailAddress) });
      var expiredResp = Creator.GetCachedUserInfoResponseExpired(resp);
      var expiredRespJson = JsonConvert.SerializeObject(expiredResp);

      _config.Setup(x => x["Jwt:UserInfo"]).Returns(UserInfoEndpoint);
      _cache.Setup(x => x.TryGetValue(It.Is<string>(token => token == BearerToken), out expiredRespJson)).Returns(true);
      _rover.Setup(x => x.GetAsync(UserInfoEndpoint, BearerToken.Substring(7))).ReturnsAsync(resp);
      _contactsDatastore.Setup(x => x.ByEmail(EmailAddress)).Returns(contact);
      _organisationDatastore.Setup(x => x.ByContact(contact.Id)).Returns(organisation);


      var auth = new BearerAuthentication(
        _cache.Object,
        _config.Object,
        _logger.Object,
        _rover.Object,
        _contactsDatastore.Object,
        _organisationDatastore.Object);
      await auth.Authenticate(_context);


      _context.Principal.Claims
        .Should()
        .NotContain(x =>
          x.Type == ClaimTypes.Role);
    }

    [Test]
    public async Task Authenticate_OrganisationPrimaryRole_ClaimsOrganisation()
    {
      var contact = Creator.GetContact();
      var organisation = Creator.GetOrganisation();
      var resp = Creator.GetUserInfoResponse(new[] { ("email", EmailAddress) });
      var expiredResp = Creator.GetCachedUserInfoResponseExpired(resp);
      var expiredRespJson = JsonConvert.SerializeObject(expiredResp);

      _config.Setup(x => x["Jwt:UserInfo"]).Returns(UserInfoEndpoint);
      _cache.Setup(x => x.TryGetValue(It.Is<string>(token => token == BearerToken), out expiredRespJson)).Returns(true);
      _rover.Setup(x => x.GetAsync(UserInfoEndpoint, BearerToken.Substring(7))).ReturnsAsync(resp);
      _contactsDatastore.Setup(x => x.ByEmail(EmailAddress)).Returns(contact);
      _organisationDatastore.Setup(x => x.ByContact(contact.Id)).Returns(organisation);


      var auth = new BearerAuthentication(
        _cache.Object,
        _config.Object,
        _logger.Object,
        _rover.Object,
        _contactsDatastore.Object,
        _organisationDatastore.Object);
      await auth.Authenticate(_context);


      _context.Principal.Claims
        .Should()
        .ContainSingle(x =>
          x.Type == nameof(Organisations));
    }

    [Test]
    public async Task Authenticate_EmailNotFound_NoClaims()
    {
      var resp = Creator.GetUserInfoResponse(new[] { ("email", EmailAddress) });
      var expiredResp = Creator.GetCachedUserInfoResponseExpired(resp);
      var expiredRespJson = JsonConvert.SerializeObject(expiredResp);

      _config.Setup(x => x["Jwt:UserInfo"]).Returns(UserInfoEndpoint);
      _cache.Setup(x => x.TryGetValue(It.Is<string>(token => token == BearerToken), out expiredRespJson)).Returns(true);
      _rover.Setup(x => x.GetAsync(UserInfoEndpoint, BearerToken.Substring(7))).ReturnsAsync(resp);


      var auth = new BearerAuthentication(
        _cache.Object,
        _config.Object,
        _logger.Object,
        _rover.Object,
        _contactsDatastore.Object,
        _organisationDatastore.Object);
      await auth.Authenticate(_context);


      _context.Principal.Claims.Should().BeEmpty();
    }

    [Test]
    public async Task Authenticate_OrganisationNotFound_NoClaims()
    {
      var contact = Creator.GetContact();
      var resp = Creator.GetUserInfoResponse(new[] { ("email", EmailAddress) });
      var expiredResp = Creator.GetCachedUserInfoResponseExpired(resp);
      var expiredRespJson = JsonConvert.SerializeObject(expiredResp);

      _config.Setup(x => x["Jwt:UserInfo"]).Returns(UserInfoEndpoint);
      _cache.Setup(x => x.TryGetValue(It.Is<string>(token => token == BearerToken), out expiredRespJson)).Returns(true);
      _rover.Setup(x => x.GetAsync(UserInfoEndpoint, BearerToken.Substring(7))).ReturnsAsync(resp);
      _contactsDatastore.Setup(x => x.ByEmail(EmailAddress)).Returns(contact);


      var auth = new BearerAuthentication(
        _cache.Object,
        _config.Object,
        _logger.Object,
        _rover.Object,
        _contactsDatastore.Object,
        _organisationDatastore.Object);
      await auth.Authenticate(_context);


      _context.Principal.Claims.Should().BeEmpty();
    }
  }
#pragma warning restore AsyncFixer01
}
;