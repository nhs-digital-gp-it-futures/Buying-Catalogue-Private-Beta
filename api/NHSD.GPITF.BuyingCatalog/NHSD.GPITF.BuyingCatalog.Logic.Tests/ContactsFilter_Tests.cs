using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class ContactsFilter_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<IOrganisationsDatastore> _organisationDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _organisationDatastore = new Mock<IOrganisationsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new ContactsFilter(_context.Object, _organisationDatastore.Object));
    }

    [TestCase(Roles.Admin)]
    [TestCase(Roles.Buyer)]
    public void Filter_NonSupplier_Returns_All(string role)
    {
      var ctx = Creator.GetContext(role: role);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var filter = new ContactsFilter(_context.Object, _organisationDatastore.Object);
      var contacts = new[]
      {
        Creator.GetContact(),
        Creator.GetContact(),
        Creator.GetContact()
      };
      var res = filter.Filter(contacts);

      res.Should().BeEquivalentTo(contacts);
    }

    [Test]
    public void Filter_Supplier_Returns_OwnNHSD()
    {
      var orgId = Guid.NewGuid().ToString();
      var org = Creator.GetOrganisation(id: orgId, primaryRoleId: PrimaryRole.ApplicationServiceProvider);

      var otherOrgId = Guid.NewGuid().ToString();
      var otherOrg = Creator.GetOrganisation(id: otherOrgId, primaryRoleId: PrimaryRole.ApplicationServiceProvider);

      var nhsdOrgId = Guid.NewGuid().ToString();
      var nhsd = Creator.GetOrganisation(id: nhsdOrgId, primaryRoleId: PrimaryRole.GovernmentDepartment);

      var cont1 = Creator.GetContact(orgId: orgId);
      var cont2 = Creator.GetContact(orgId: orgId);
      var cont3 = Creator.GetContact(orgId: otherOrgId);
      var cont4 = Creator.GetContact(orgId: nhsdOrgId);

      _organisationDatastore.Setup(x => x.ByContact(cont1.Id)).Returns(org);
      _organisationDatastore.Setup(x => x.ByContact(cont2.Id)).Returns(org);
      _organisationDatastore.Setup(x => x.ByContact(cont3.Id)).Returns(otherOrg);
      _organisationDatastore.Setup(x => x.ByContact(cont4.Id)).Returns(nhsd);

      var ctx = Creator.GetContext(orgId: orgId, role: Roles.Supplier);
      _context.Setup(c => c.HttpContext).Returns(ctx);

      var filter = new ContactsFilter(_context.Object, _organisationDatastore.Object);
      var contacts = new[] { cont1, cont2, cont3, cont4 };


      var res = filter.Filter(contacts);


      res.Should().BeEquivalentTo(new[] { cont1, cont2, cont4 });
    }
  }
}
