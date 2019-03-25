using FluentAssertions;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class StandardsApplicableModifier_Tests
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new StandardsApplicableModifier());
    }

    [Test]
    public void ForUpdate_ToSubmitted_Sets_SubmittedOn_ToUtcNow()
    {
      var modifier = new StandardsApplicableModifier();
      var submittedOn = new DateTime(2006, 2, 20, 6, 3, 0);
      var claim = Creator.GetStandardsApplicable(status: StandardsApplicableStatus.Submitted, submittedOn: submittedOn);

      modifier.ForUpdate(claim);

      claim.SubmittedOn.Should().BeCloseTo(DateTime.UtcNow);
    }

    [TestCase(StandardsApplicableStatus.NotStarted)]
    [TestCase(StandardsApplicableStatus.Draft)]
    [TestCase(StandardsApplicableStatus.Remediation)]
    [TestCase(StandardsApplicableStatus.Approved)]
    [TestCase(StandardsApplicableStatus.ApprovedFirstOfType)]
    [TestCase(StandardsApplicableStatus.ApprovedPartial)]
    [TestCase(StandardsApplicableStatus.Rejected)]
    public void ForUpdate_NotSubmitted_DoesNotSet_SubmittedOn(StandardsApplicableStatus status)
    {
      var modifier = new StandardsApplicableModifier();
      var submittedOn = new DateTime(2006, 2, 20, 6, 3, 0);
      var claim = Creator.GetStandardsApplicable(status: status, submittedOn: submittedOn);

      modifier.ForUpdate(claim);

      claim.SubmittedOn.Should().BeCloseTo(submittedOn);
    }

    [Test]
    public void ForUpdate_DefaultSubmittedOn_Sets_SubmittedOn_ToUnixEpoch()
    {
      var modifier = new StandardsApplicableModifier();
      var submittedOn = default(DateTime);
      var claim = Creator.GetStandardsApplicable(submittedOn: submittedOn);

      modifier.ForUpdate(claim);

      claim.SubmittedOn.Should().BeCloseTo(DateTime.UnixEpoch);
    }

    [Test]
    public void ForUpdate_SubmittedOn_DoesNotSet_SubmittedOn()
    {
      var modifier = new StandardsApplicableModifier();
      var submittedOn = new DateTime(2006, 2, 20, 6, 3, 0);
      var claim = Creator.GetStandardsApplicable(submittedOn: submittedOn);

      modifier.ForUpdate(claim);

      claim.SubmittedOn.Should().BeCloseTo(submittedOn);
    }

    [Test]
    public void ForUpdate_DefaultAssignedOn_Sets_SubmittedOn_ToUnixEpoch()
    {
      var modifier = new StandardsApplicableModifier();
      var assignedOn = default(DateTime);
      var claim = Creator.GetStandardsApplicable(assignedOn: assignedOn);

      modifier.ForUpdate(claim);

      claim.AssignedOn.Should().BeCloseTo(DateTime.UnixEpoch);
    }

    [Test]
    public void ForUpdate_AssignedOn_DoesNotSet_SubmittedOn()
    {
      var modifier = new StandardsApplicableModifier();
      var assignedOn = new DateTime(2006, 2, 20, 6, 3, 0);
      var claim = Creator.GetStandardsApplicable(assignedOn: assignedOn);

      modifier.ForUpdate(claim);

      claim.AssignedOn.Should().BeCloseTo(assignedOn);
    }
  }
}
