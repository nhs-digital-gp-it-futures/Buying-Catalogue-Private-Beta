using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class LinkManagerValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<LinkManagerValidator>> _logger;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<LinkManagerValidator>>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new LinkManagerValidator(_context.Object, _logger.Object));
    }
  }
}
