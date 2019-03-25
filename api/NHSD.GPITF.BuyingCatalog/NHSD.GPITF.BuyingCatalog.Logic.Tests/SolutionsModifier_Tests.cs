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
  public sealed class SolutionsModifier_Tests
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
      Assert.DoesNotThrow(() => new SolutionsModifier(_context.Object, _contacts.Object));
    }

    [Test]
    public void ForCreate_Sets_CreatedById()
    {
      var modifier = new SolutionsModifier(_context.Object, _contacts.Object);
      var soln = Creator.GetSolution();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contactId = Guid.NewGuid().ToString();
      var contact = Creator.GetContact(id: contactId);
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);

      modifier.ForCreate(soln);

      soln.CreatedById.Should().Be(contactId);
    }

    [Test]
    public void ForCreate_Sets_CreatedByOn()
    {
      var modifier = new SolutionsModifier(_context.Object, _contacts.Object);
      var soln = Creator.GetSolution();
      soln.ModifiedOn = new DateTime(2006, 2, 20);
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contact = Creator.GetContact();
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);

      modifier.ForCreate(soln);

      soln.CreatedOn.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForCreate_Sets_ModifiedById()
    {
      var modifier = new SolutionsModifier(_context.Object, _contacts.Object);
      var soln = Creator.GetSolution();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contactId = Guid.NewGuid().ToString();
      var contact = Creator.GetContact(id: contactId);
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);

      modifier.ForCreate(soln);

      soln.ModifiedById.Should().Be(contactId);
    }

    [Test]
    public void ForCreate_Sets_ModifiedByOn()
    {
      var modifier = new SolutionsModifier(_context.Object, _contacts.Object);
      var soln = Creator.GetSolution();
      soln.ModifiedOn = new DateTime(2006, 2, 20);
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contact = Creator.GetContact();
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);

      modifier.ForCreate(soln);

      soln.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow);
    }

    [Test]
    public void ForUpdate_Sets_ModifiedById()
    {
      var modifier = new SolutionsModifier(_context.Object, _contacts.Object);
      var soln = Creator.GetSolution();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contactId = Guid.NewGuid().ToString();
      var contact = Creator.GetContact(id: contactId);
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);

      modifier.ForUpdate(soln);

      soln.ModifiedById.Should().Be(contactId);
    }

    [Test]
    public void ForUpdate_Sets_ModifiedByOn()
    {
      var modifier = new SolutionsModifier(_context.Object, _contacts.Object);
      var soln = Creator.GetSolution();
      soln.ModifiedOn = new DateTime(2006, 2, 20);
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var contact = Creator.GetContact();
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(contact);

      modifier.ForUpdate(soln);

      soln.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow);
    }
  }
}
