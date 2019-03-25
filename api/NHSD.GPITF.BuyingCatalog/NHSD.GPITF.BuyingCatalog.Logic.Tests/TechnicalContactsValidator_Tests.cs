using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class TechnicalContactsValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<TechnicalContactsValidator>> _logger;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger<TechnicalContactsValidator>>();
      _context = new Mock<IHttpContextAccessor>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new TechnicalContactsValidator(_context.Object, _logger.Object, _solutionDatastore.Object));
    }

    [Test]
    public void SupplierOwn_Own_Succeeds()
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(orgId: orgId, role: Roles.Supplier);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var soln = Creator.GetSolution(orgId: orgId);
      var techCont = Creator.GetTechnicalContact();
      _solutionDatastore.Setup(x => x.ById(techCont.SolutionId)).Returns(soln);
      var validator = new TechnicalContactsValidator(_context.Object, _logger.Object, _solutionDatastore.Object);

      validator.SupplierOwn();
      var res = validator.Validate(techCont);

      res.Errors.Should().BeEmpty();
    }

    [Test]
    public void SupplierOwn_Other_ReturnsError()
    {
      var orgId = Guid.NewGuid().ToString();
      var ctx = Creator.GetContext(orgId: orgId, role: Roles.Supplier);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      var soln = Creator.GetSolution();
      var techCont = Creator.GetTechnicalContact();
      _solutionDatastore.Setup(x => x.ById(techCont.SolutionId)).Returns(soln);
      var validator = new TechnicalContactsValidator(_context.Object, _logger.Object, _solutionDatastore.Object);

      validator.SupplierOwn();
      var res = validator.Validate(techCont);

      res.Errors.Count().Should().Be(1);
    }
  }
}
