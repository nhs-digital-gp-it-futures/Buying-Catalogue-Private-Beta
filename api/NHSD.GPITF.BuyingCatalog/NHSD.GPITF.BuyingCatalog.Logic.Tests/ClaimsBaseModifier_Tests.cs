using FluentAssertions;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class ClaimsBaseModifier_Tests
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyClaimsBaseModifier());
    }

    [Test]
    public void ForCreate_SetsOriginalDate_ToUtcNow()
    {
      var modifier = new DummyClaimsBaseModifier();
      var claim = Creator.GetClaimsBase(originalDate: DateTime.MinValue);

      modifier.ForCreate(claim);

      claim.OriginalDate.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForUpdate_DefaultOriginalDate_SetsOriginalDate_ToUtcNow()
    {
      var modifier = new DummyClaimsBaseModifier();
      var claim = Creator.GetClaimsBase(originalDate: default(DateTime));

      modifier.ForUpdate(claim);

      claim.OriginalDate.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForUpdate_OriginalDate_DoesNotSet_OriginalDate()
    {
      var modifier = new DummyClaimsBaseModifier();
      var originalDate = new DateTime(2006, 2, 20, 6, 3, 0);
      var claim = Creator.GetClaimsBase(originalDate: originalDate);

      modifier.ForUpdate(claim);

      claim.OriginalDate.Should().BeCloseTo(originalDate);
    }
  }
}
