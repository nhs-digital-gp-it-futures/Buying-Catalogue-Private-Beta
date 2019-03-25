using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class ReviewsLogicBase_Tests
  {
    private Mock<IReviewsBaseModifier<ReviewsBase>> _modifier;
    private Mock<IReviewsDatastore<ReviewsBase>> _datastore;
    private Mock<IContactsDatastore> _contacts;
    private Mock<IReviewsValidator<ReviewsBase>> _validator;
    private Mock<IReviewsFilter<IEnumerable<ReviewsBase>>> _filter;
    private Mock<IHttpContextAccessor> _context;

    [SetUp]
    public void SetUp()
    {
      _modifier = new Mock<IReviewsBaseModifier<ReviewsBase>>();
      _datastore = new Mock<IReviewsDatastore<ReviewsBase>>();
      _contacts = new Mock<IContactsDatastore>();
      _validator = new Mock<IReviewsValidator<ReviewsBase>>();
      _filter = new Mock<IReviewsFilter<IEnumerable<ReviewsBase>>>();
      _context = new Mock<IHttpContextAccessor>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyReviewsLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object));
    }

    [Test]
    public void ByEvidence_CallsFilter()
    {
      var logic = new DummyReviewsLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object);

      logic.ByEvidence("some Id");

      _filter.Verify(x => x.Filter(It.IsAny<IEnumerable<IEnumerable<ReviewsBase>>>()), Times.Once());
    }

    [Test]
    public void Create_CallsValidator_WithRuleset()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var logic = new DummyReviewsLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object);
      var review = Creator.GetReviewsBase();

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Create(review);

      _validator.Verify(x => x.ValidateAndThrowEx(
        It.Is<DummyReviewsBase>(ev => ev == review),
        It.Is<string>(rs => rs == nameof(IEvidenceLogic<ReviewsLogicBase<ReviewsBase>>.Create))), Times.Once());
    }

    [Test]
    public void Create_Calls_Modifier()
    {
      var logic = new DummyReviewsLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object);
      var review = Creator.GetReviewsBase(originalDate: DateTime.MinValue);

      logic.Create(review);

      _modifier.Verify(x => x.ForCreate(review), Times.Once);
    }
  }
}
