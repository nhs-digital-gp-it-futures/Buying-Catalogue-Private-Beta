using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class OrganisationsFilter_Tests
  {
    private Mock<IHttpContextAccessor> _context;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new OrganisationsFilter(_context.Object));
    }

    [Test]
    public void Filter_Admin_Returns_All()
    {
      var suppOrgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(orgId: suppOrgId, role: Roles.Admin);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var orgFilt = new OrganisationsFilter(_context.Object);
      var govOrg = Creator.GetOrganisation(id: Guid.NewGuid().ToString(), primaryRoleId: PrimaryRole.GovernmentDepartment);
      var supp1Org = Creator.GetOrganisation(id: suppOrgId, primaryRoleId: PrimaryRole.ApplicationServiceProvider);
      var supp2Org = Creator.GetOrganisation(id: Guid.NewGuid().ToString(), primaryRoleId: PrimaryRole.ApplicationServiceProvider);
      var orgs = new[] { govOrg, supp1Org, supp2Org };

      var filterOrg = orgFilt.Filter(orgs.ToList());

      filterOrg.Should().BeEquivalentTo(orgs);
    }

    [Test]
    public void Filter_Buyer_Returns_All()
    {
      var suppOrgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(orgId: suppOrgId, role: Roles.Buyer);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var orgFilt = new OrganisationsFilter(_context.Object);
      var govOrg = Creator.GetOrganisation(id:Guid.NewGuid().ToString(), primaryRoleId: PrimaryRole.GovernmentDepartment);
      var supp1Org = Creator.GetOrganisation(id:suppOrgId, primaryRoleId: PrimaryRole.ApplicationServiceProvider);
      var supp2Org = Creator.GetOrganisation(id: Guid.NewGuid().ToString(), primaryRoleId: PrimaryRole.ApplicationServiceProvider);
      var orgs = new[] { govOrg, supp1Org, supp2Org };

      var filterOrg = orgFilt.Filter(orgs.ToList());

      filterOrg.Should().BeEquivalentTo(orgs);
    }

    [Test]
    public void Filter_Supplier_Returns_NotOther()
    {
      var suppOrgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(orgId: suppOrgId, role: Roles.Supplier);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var orgFilt = new OrganisationsFilter(_context.Object);
      var govOrg = Creator.GetOrganisation(id: Guid.NewGuid().ToString(), primaryRoleId: PrimaryRole.GovernmentDepartment);
      var supp1Org = Creator.GetOrganisation(id: suppOrgId, primaryRoleId: PrimaryRole.ApplicationServiceProvider);
      var supp2Org = Creator.GetOrganisation(id: Guid.NewGuid().ToString(), primaryRoleId: PrimaryRole.ApplicationServiceProvider);
      var orgs = new[] { govOrg, supp1Org, supp2Org };

      var filterOrg = orgFilt.Filter(orgs.ToList());

      filterOrg.Should().BeEquivalentTo(new[] { govOrg, supp1Org });
    }
  }
}
