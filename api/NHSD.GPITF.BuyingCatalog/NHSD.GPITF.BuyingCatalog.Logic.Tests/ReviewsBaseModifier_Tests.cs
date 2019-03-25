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
  public sealed class ReviewsBaseModifier_Tests
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
      Assert.DoesNotThrow(() => new DummyReviewsBaseModifier(_context.Object, _contacts.Object));
    }

    [Test]
    public void ForCreate_SetsOriginalDate_ToUtcNow()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyReviewsBaseModifier(_context.Object, _contacts.Object);
      var review = Creator.GetReviewsBase(originalDate: DateTime.MinValue);

      modifier.ForCreate(review);

      review.OriginalDate.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForCreate_SetsCreatedOn_ToUtcNow()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyReviewsBaseModifier(_context.Object, _contacts.Object);
      var review = Creator.GetReviewsBase(createdOn: DateTime.MinValue);

      modifier.ForCreate(review);

      review.CreatedOn.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForCreate_SetsCreatedById()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contactId = Guid.NewGuid().ToString();
      var contact = Creator.GetContact(id: contactId);
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);
      var modifier = new DummyReviewsBaseModifier(_context.Object, _contacts.Object);
      var review = Creator.GetReviewsBase(originalDate: DateTime.MinValue);

      modifier.ForCreate(review);

      review.CreatedById.Should().Be(contactId);
    }

    [Test]
    public void ForUpdate_DefaultOriginalDate_SetsOriginalDate_ToUtcNow()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyReviewsBaseModifier(_context.Object, _contacts.Object);
      var review = Creator.GetReviewsBase(originalDate: default(DateTime));

      modifier.ForUpdate(review);

      review.OriginalDate.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForUpdate_OriginalDate_DoesNotSet_OriginalDate()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var modifier = new DummyReviewsBaseModifier(_context.Object, _contacts.Object);
      var originalDate = new DateTime(2006, 2, 20, 6, 3, 0);
      var review = Creator.GetReviewsBase(originalDate: originalDate);

      modifier.ForUpdate(review);

      review.OriginalDate.Should().BeCloseTo(originalDate);
    }
  }
}
