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
  public sealed class EvidenceBaseModifier_Tests
  {
    private Mock<IContactsDatastore> _contacts;
    private Mock<IHttpContextAccessor> _context;

    [SetUp]
    public void SetUp()
    {
      _contacts = new Mock<IContactsDatastore>();
      _context = new Mock<IHttpContextAccessor>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyEvidenceBaseModifier(_context.Object, _contacts.Object));
    }

    [Test]
    public void ForCreate_SetsOriginalDate_ToUtcNow()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyEvidenceBaseModifier(_context.Object, _contacts.Object);
      var evidence = Creator.GetEvidenceBase(originalDate: DateTime.MinValue);

      modifier.ForCreate(evidence);

      evidence.OriginalDate.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForCreate_SetsCreatedOn_ToUtcNow()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyEvidenceBaseModifier(_context.Object, _contacts.Object);
      var evidence = Creator.GetEvidenceBase(createdOn: DateTime.MinValue);

      modifier.ForCreate(evidence);

      evidence.CreatedOn.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForCreate_SetsCreatedById()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contactId = Guid.NewGuid().ToString();
      var contact = Creator.GetContact(id: contactId);
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);
      var modifier = new DummyEvidenceBaseModifier(_context.Object, _contacts.Object);
      var evidence = Creator.GetEvidenceBase(originalDate: DateTime.MinValue);

      modifier.ForCreate(evidence);

      evidence.CreatedById.Should().Be(contactId);
    }

    [Test]
    public void ForUpdate_DefaultOriginalDate_SetsOriginalDate_ToUtcNow()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyEvidenceBaseModifier(_context.Object, _contacts.Object);
      var evidence = Creator.GetEvidenceBase(originalDate: default(DateTime));

      modifier.ForUpdate(evidence);

      evidence.OriginalDate.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForUpdate_OriginalDate_DoesNotSet_OriginalDate()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyEvidenceBaseModifier(_context.Object, _contacts.Object);
      var originalDate = new DateTime(2006, 2, 20, 6, 3, 0);
      var evidence = Creator.GetEvidenceBase(originalDate: originalDate);

      modifier.ForUpdate(evidence);

      evidence.OriginalDate.Should().BeCloseTo(originalDate);
    }
  }
}
