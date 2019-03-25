using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class FilterBase_Tests
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
      Assert.DoesNotThrow(() => new DummyFilterBase(_context.Object));
    }

    [Test]
    public void Filter_Null_ReturnsEmpty()
    {
      object obj = null;
      var filter = new DummyFilterBase(_context.Object);

      var res = filter.Filter(new[] { obj });

      res.Should().BeEmpty();
    }
  }
}
