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
  public sealed class EvidenceLogicBase_Tests
  {
    private Mock<IEvidenceBaseModifier<EvidenceBase>> _modifier;
    private Mock<IEvidenceDatastore<EvidenceBase>> _datastore;
    private Mock<IContactsDatastore> _contacts;
    private Mock<IEvidenceValidator<EvidenceBase>> _validator;
    private Mock<IEvidenceFilter<IEnumerable<EvidenceBase>>> _filter;
    private Mock<IHttpContextAccessor> _context;

    [SetUp]
    public void SetUp()
    {
      _modifier = new Mock<IEvidenceBaseModifier<EvidenceBase>>();
      _datastore = new Mock<IEvidenceDatastore<EvidenceBase>>();
      _contacts = new Mock<IContactsDatastore>();
      _validator = new Mock<IEvidenceValidator<EvidenceBase>>();
      _filter = new Mock<IEvidenceFilter<IEnumerable<EvidenceBase>>>();
      _context = new Mock<IHttpContextAccessor>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyEvidenceLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object));
    }

    [Test]
    public void ByClaim_CallsFilter()
    {
      var logic = new DummyEvidenceLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object);

      logic.ByClaim("some Id");

      _filter.Verify(x => x.Filter(It.IsAny<IEnumerable<IEnumerable<EvidenceBase>>>()), Times.Once());
    }

    [Test]
    public void Create_CallsValidator_WithRuleset()
    {
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());
      var logic = new DummyEvidenceLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object);
      var evidence = Creator.GetEvidenceBase();

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Create(evidence);

      _validator.Verify(x => x.ValidateAndThrowEx(
        It.Is<DummyEvidenceBase>(ev => ev == evidence),
        It.Is<string>(rs => rs == nameof(IEvidenceLogic<EvidenceLogicBase<EvidenceBase>>.Create))), Times.Once());
    }

    [Test]
    public void Create_Calls_Modifier()
    {
      var logic = new DummyEvidenceLogicBase(_modifier.Object, _datastore.Object, _contacts.Object, _validator.Object, _filter.Object, _context.Object);
      var evidence = Creator.GetEvidenceBase(originalDate: DateTime.MinValue);

      logic.Create(evidence);

      _modifier.Verify(x => x.ForCreate(evidence), Times.Once);
    }
  }
}
