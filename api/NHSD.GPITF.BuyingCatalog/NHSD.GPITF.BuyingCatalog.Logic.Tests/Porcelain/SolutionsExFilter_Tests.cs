using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Logic.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests.Porcelain
{
  [TestFixture]
  public sealed class SolutionsExFilter_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ISolutionsFilter> _solnFilter;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _solnFilter = new Mock<ISolutionsFilter>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new SolutionsExFilter(_context.Object, _solnFilter.Object));
    }

    [Test]
    public void Filter_Null_CallsSolutionsFilterWithNull()
    {
      var filter = new SolutionsExFilter(_context.Object, _solnFilter.Object);

      filter.Filter((SolutionEx)null);

      _solnFilter.Verify(x => x.Filter(It.Is<IEnumerable<Solutions>>(arr => arr.All(soln => soln == null))));
    }
  }
}
