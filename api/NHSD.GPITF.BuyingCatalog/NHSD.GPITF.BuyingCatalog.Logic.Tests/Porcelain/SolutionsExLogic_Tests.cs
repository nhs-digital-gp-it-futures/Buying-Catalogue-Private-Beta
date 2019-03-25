using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Logic.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests.Porcelain
{
  [TestFixture]
  public sealed class SolutionsExLogic_Tests
  {
    private Mock<ISolutionsModifier> _solutionsModifier;

    private Mock<ICapabilitiesImplementedModifier> _capabilitiesImplementedModifier;
    private Mock<IStandardsApplicableModifier> _standardsApplicableModifier;

    private Mock<ICapabilitiesImplementedEvidenceModifier> _capabilitiesImplementedEvidenceModifier;
    private Mock<IStandardsApplicableEvidenceModifier> _standardsApplicableEvidenceModifier;

    private Mock<ICapabilitiesImplementedReviewsModifier> _capabilitiesImplementedReviewsModifier;
    private Mock<IStandardsApplicableReviewsModifier> _standardsApplicableReviewsModifier;

    private Mock<ISolutionsExDatastore> _datastore;
    private Mock<IContactsDatastore> _contacts;
    private Mock<IHttpContextAccessor> _context;
    private Mock<ISolutionsExValidator> _validator;
    private Mock<ISolutionsExFilter> _filter;
    private Mock<IEvidenceBlobStoreLogic> _evidenceBlobStoreLogic;

    [SetUp]
    public void SetUp()
    {
      _solutionsModifier = new Mock<ISolutionsModifier>();

      _capabilitiesImplementedModifier = new Mock<ICapabilitiesImplementedModifier>();
      _standardsApplicableModifier = new Mock<IStandardsApplicableModifier>();

      _capabilitiesImplementedEvidenceModifier = new Mock<ICapabilitiesImplementedEvidenceModifier>();
      _standardsApplicableEvidenceModifier = new Mock<IStandardsApplicableEvidenceModifier>();

      _capabilitiesImplementedReviewsModifier = new Mock<ICapabilitiesImplementedReviewsModifier>();
      _standardsApplicableReviewsModifier = new Mock<IStandardsApplicableReviewsModifier>();

      _datastore = new Mock<ISolutionsExDatastore>();
      _contacts = new Mock<IContactsDatastore>();
      _context = new Mock<IHttpContextAccessor>();
      _validator = new Mock<ISolutionsExValidator>();
      _filter = new Mock<ISolutionsExFilter>();
      _evidenceBlobStoreLogic = new Mock<IEvidenceBlobStoreLogic>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object));
    }

    [Test]
    public void Update_CallsValidator_WithRuleset()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var solnEx = Creator.GetSolutionEx();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _validator.Verify(x => x.ValidateAndThrowEx(
        It.Is<SolutionEx>(sex => sex == solnEx),
        It.Is<string>(rs => rs == nameof(ISolutionsExLogic.Update))), Times.Once());
    }

    [TestCase(SolutionStatus.Registered)]
    public void Update_CallsPrepareForSolution_WhenRegistered(SolutionStatus status)
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var soln = Creator.GetSolution(status: status);
      var solnEx = Creator.GetSolutionEx(soln: soln);
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _evidenceBlobStoreLogic.Verify(x => x.PrepareForSolution(soln.Id), Times.Once);
    }

    [TestCase(SolutionStatus.Failed)]
    [TestCase(SolutionStatus.Draft)]
    [TestCase(SolutionStatus.CapabilitiesAssessment)]
    [TestCase(SolutionStatus.StandardsCompliance)]
    [TestCase(SolutionStatus.FinalApproval)]
    [TestCase(SolutionStatus.SolutionPage)]
    [TestCase(SolutionStatus.Approved)]
    public void Update_DoesNotCallPrepareForSolution_WhenNotRegistered(SolutionStatus status)
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var soln = Creator.GetSolution(status: status);
      var solnEx = Creator.GetSolutionEx(soln: soln);
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      _contacts.Setup(x => x.ByEmail(It.IsAny<string>())).Returns(Creator.GetContact());

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _evidenceBlobStoreLogic.Verify(x => x.PrepareForSolution(soln.Id), Times.Never);
    }

    [Test]
    public void Update_Calls_SolutionModifier()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var soln = Creator.GetSolution();
      var solnEx = Creator.GetSolutionEx(soln: soln);

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _solutionsModifier.Verify(x => x.ForUpdate(soln), Times.Once);
    }

    [Test]
    public void Update_Calls_Modifier_For_ClaimedCapability()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var claim = Creator.GetCapabilitiesImplemented();
      var soln = Creator.GetSolution();
      var solnEx = Creator.GetSolutionEx(soln: soln, claimedCap: new List<CapabilitiesImplemented>(new[] { claim }));

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _capabilitiesImplementedModifier.Verify(x => x.ForUpdate(claim), Times.Once);
    }

    [Test]
    public void Update_Calls_Modifier_For_ClaimedStandard()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var claim = Creator.GetStandardsApplicable();
      var soln = Creator.GetSolution();
      var solnEx = Creator.GetSolutionEx(soln: soln, claimedStd: new List<StandardsApplicable>(new[] { claim }));

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _standardsApplicableModifier.Verify(x => x.ForUpdate(claim), Times.Once);
    }

    [Test]
    public void Update_Calls_Modifier_For_ClaimedCapabilityEvidence()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var evidence = Creator.GetCapabilitiesImplementedEvidence();
      var soln = Creator.GetSolution();
      var solnEx = Creator.GetSolutionEx(soln: soln, claimedCapEv: new List<CapabilitiesImplementedEvidence>(new[] { evidence }));

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _capabilitiesImplementedEvidenceModifier.Verify(x => x.ForUpdate(evidence), Times.Once);
    }

    [Test]
    public void Update_Calls_Modifier_For_ClaimedStandardEvidence()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var evidence = Creator.GetStandardsApplicableEvidence();
      var soln = Creator.GetSolution();
      var solnEx = Creator.GetSolutionEx(soln: soln, claimedStdEv: new List<StandardsApplicableEvidence>(new[] { evidence }));

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _standardsApplicableEvidenceModifier.Verify(x => x.ForUpdate(evidence), Times.Once);
    }

    [Test]
    public void Update_Calls_Modifier_For_ClaimedCapabilityReview()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var review = Creator.GetCapabilitiesImplementedReviews();
      var soln = Creator.GetSolution();
      var solnEx = Creator.GetSolutionEx(soln: soln, claimedCapRev: new List<CapabilitiesImplementedReviews>(new[] { review }));

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _capabilitiesImplementedReviewsModifier.Verify(x => x.ForUpdate(review), Times.Once);
    }

    [Test]
    public void Update_Calls_Modifier_For_ClaimedStandardReview()
    {
      var logic = new SolutionsExLogic(
        _solutionsModifier.Object,
        _capabilitiesImplementedModifier.Object,
        _standardsApplicableModifier.Object,
        _capabilitiesImplementedEvidenceModifier.Object,
        _standardsApplicableEvidenceModifier.Object,
        _capabilitiesImplementedReviewsModifier.Object,
        _standardsApplicableReviewsModifier.Object,
        _datastore.Object, _context.Object, _validator.Object, _filter.Object,
        _contacts.Object, _evidenceBlobStoreLogic.Object);
      var review = Creator.GetStandardsApplicableReviews();
      var soln = Creator.GetSolution();
      var solnEx = Creator.GetSolutionEx(soln: soln, claimedStdRev: new List<StandardsApplicableReviews>(new[] { review }));

      var valres = new ValidationResult();
      _validator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(valres);

      logic.Update(solnEx);

      _standardsApplicableReviewsModifier.Verify(x => x.ForUpdate(review), Times.Once);
    }
  }
}
