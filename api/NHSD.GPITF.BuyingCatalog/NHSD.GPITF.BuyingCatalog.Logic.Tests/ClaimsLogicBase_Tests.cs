using FluentAssertions;
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
  public sealed class ClaimsLogicBase_Tests
  {
    private Mock<IClaimsBaseModifier<ClaimsBase>> _modifier;
    private Mock<IHttpContextAccessor> _context;
    private Mock<IClaimsDatastore<ClaimsBase>> _datastore;
    private Mock<IClaimsValidator<ClaimsBase>> _validator;
    private Mock<IClaimsFilter<ClaimsBase>> _filter;

    [SetUp]
    public void SetUp()
    {
      _modifier = new Mock<IClaimsBaseModifier<ClaimsBase>>();
      _context = new Mock<IHttpContextAccessor>();
      _datastore = new Mock<IClaimsDatastore<ClaimsBase>>();
      _validator = new Mock<IClaimsValidator<ClaimsBase>>();
      _filter = new Mock<IClaimsFilter<ClaimsBase>>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyClaimsLogicBase(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object));
    }

    [Test]
    public void Create_CallsValidator_WithRuleset()
    {
      var logic = new DummyClaimsLogicBase(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);
      var claim = Creator.GetClaimsBase();

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Create(claim);

      _validator.Verify(x => x.ValidateAndThrowEx(
        It.Is<ClaimsBase>(c => c == claim),
        It.Is<string>(rs => rs == nameof(IClaimsLogic<ClaimsBase>.Create))), Times.Once());
    }

    [Test]
    public void Create_Calls_Modifier()
    {
      var logic = new DummyClaimsLogicBase(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);
      var claim = Creator.GetClaimsBase(originalDate: DateTime.MinValue);
      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Create(claim);

      _modifier.Verify(x => x.ForCreate(claim), Times.Once);
    }

    [Test]
    public void Update_CallsValidator_WithRuleset()
    {
      var logic = new DummyClaimsLogicBase(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);
      var claim = Creator.GetClaimsBase();

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(claim);

      _validator.Verify(x => x.ValidateAndThrowEx(
        It.Is<ClaimsBase>(c => c == claim),
        It.Is<string>(rs => rs == nameof(IClaimsLogic<ClaimsBase>.Update))), Times.Once());
    }

    [Test]
    public void Delete_CallsValidator_WithRuleset()
    {
      var logic = new DummyClaimsLogicBase(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);
      var claim = Creator.GetClaimsBase();

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Delete(claim);

      _validator.Verify(x => x.ValidateAndThrowEx(
        It.Is<ClaimsBase>(c => c == claim),
        It.Is<string>(rs => rs == nameof(IClaimsLogic<ClaimsBase>.Delete))), Times.Once());
    }

    [Test]
    public void ById_CallsFilter()
    {
      var logic = new DummyClaimsLogicBase(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);

      logic.ById("some Id");

      _filter.Verify(x => x.Filter(It.IsAny<IEnumerable<ClaimsBase>>()), Times.Once());
    }

    [Test]
    public void BySolution_CallsFilter()
    {
      var logic = new DummyClaimsLogicBase(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);

      logic.BySolution("some Id");

      _filter.Verify(x => x.Filter(It.IsAny<IEnumerable<ClaimsBase>>()), Times.Once());
    }
  }
}
