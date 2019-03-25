using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class ClaimsFilterBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyClaimsFilterBase(_context.Object, _solutionDatastore.Object));
    }

    [Test]
    public void Filter_Admin_ReturnsAll()
    {
      var filter = new DummyClaimsFilterBase(_context.Object, _solutionDatastore.Object);
      var claim = Creator.GetClaimsBase();

      var res = filter.FilterForAdmin(claim);

      res.Should().Be(claim);
    }

    [Test]
    public void Filter_Buyer_ReturnsAll()
    {
      var filter = new DummyClaimsFilterBase(_context.Object, _solutionDatastore.Object);
      var claim = Creator.GetClaimsBase();

      var res = filter.FilterForBuyer(claim);

      res.Should().Be(claim);
    }

    [Test]
    public void Filter_SupplierOwn_ReturnsOwn()
    {
      var filter = new DummyClaimsFilterBase(_context.Object, _solutionDatastore.Object);
      var orgId = Guid.NewGuid().ToString();
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var ctx = Creator.GetContext(orgId: orgId);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      var res = filter.FilterForSupplier(claim);

      res.Should().Be(claim);
    }

    [Test]
    public void Filter_SupplierOther_ReturnsNull()
    {
      var filter = new DummyClaimsFilterBase(_context.Object, _solutionDatastore.Object);
      var orgId = Guid.NewGuid().ToString();
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var ctx = Creator.GetContext();
      _context.Setup(c => c.HttpContext).Returns(ctx);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      var res = filter.FilterForSupplier(claim);

      res.Should().BeNull();
    }
  }
}
