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
  public sealed class StandardsApplicableLogic_Tests
  {
    private Mock<IStandardsApplicableModifier> _modifier;
    private Mock<IHttpContextAccessor> _context;
    private Mock<IStandardsApplicableDatastore> _datastore;
    private Mock<IStandardsApplicableValidator> _validator;
    private Mock<IStandardsApplicableFilter> _filter;

    [SetUp]
    public void SetUp()
    {
      _modifier = new Mock<IStandardsApplicableModifier>();
      _context = new Mock<IHttpContextAccessor>();
      _datastore = new Mock<IStandardsApplicableDatastore>();
      _validator = new Mock<IStandardsApplicableValidator>();
      _filter    = new Mock<IStandardsApplicableFilter>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new StandardsApplicableLogic(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object));
    }

    [Test]
    public void Update_CallsValidator_WithRuleset()
    {
      var logic = new StandardsApplicableLogic(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);
      var claim = Creator.GetStandardsApplicable();

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(claim);

      _validator.Verify(x => x.ValidateAndThrowEx(
        It.Is<StandardsApplicable>(c => c == claim),
        It.Is<string>(rs => rs == nameof(IClaimsLogic<StandardsApplicable>.Update))), Times.Once());
    }


    public static IEnumerable<StandardsApplicableStatus> Statuses()
    {
      return (StandardsApplicableStatus[])Enum.GetValues(typeof(StandardsApplicableStatus));
    }

    [Test]
    public void Update_Calls_Modifier([ValueSource(nameof(Statuses))]StandardsApplicableStatus status)
    {
      var logic = new StandardsApplicableLogic(_modifier.Object, _datastore.Object, _validator.Object, _filter.Object, _context.Object);
      var claim = Creator.GetStandardsApplicable(status: status);
      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(claim);

      _modifier.Verify(x => x.ForUpdate(claim), Times.Once);
    }
  }
}
