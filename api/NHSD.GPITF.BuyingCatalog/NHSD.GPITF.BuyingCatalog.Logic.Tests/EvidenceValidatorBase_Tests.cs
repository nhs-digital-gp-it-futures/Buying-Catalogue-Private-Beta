using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class EvidenceValidatorBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<ILogger<DummyEvidenceValidatorBase>> _logger;
    private Mock<IEvidenceDatastore<EvidenceBase>> _evidenceDatastore;
    private Mock<IClaimsDatastore<ClaimsBase>> _claimDatastore;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _logger = new Mock<ILogger<DummyEvidenceValidatorBase>>();
      _evidenceDatastore = new Mock<IEvidenceDatastore<EvidenceBase>>();
      _claimDatastore = new Mock<IClaimsDatastore<ClaimsBase>>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object));
    }

    [Test]
    public void MustBeValidClaimId_Valid_Succeeds()
    {
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase();

      validator.MustBeValidClaimId();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidClaimId_Null_ReturnsError()
    {
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase();
      evidence.ClaimId = null;

      validator.MustBeValidClaimId();
      var valres = validator.Validate(evidence);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid ClaimId")
        .And
        .ContainSingle(x => x.ErrorMessage == "'Claim Id' must not be empty.")
        .And
        .HaveCount(2);
    }

    [Test]
    public void MustBeValidClaimId_NotGuid_ReturnsError()
    {
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase(claimId: "some other Id");

      validator.MustBeValidClaimId();
      var valres = validator.Validate(evidence);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid ClaimId")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeFromSameOrganisation_Same_Succeeds()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(orgId: orgId));
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var evidence = Creator.GetEvidenceBase();
      _claimDatastore.Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.MustBeFromSameOrganisation();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeFromSameOrganisation_Other_ReturnsError()
    {
      var orgId = Guid.NewGuid().ToString();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext());
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var evidence = Creator.GetEvidenceBase();
      _claimDatastore.Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      validator.MustBeFromSameOrganisation();
      var valres = validator.Validate(evidence);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Must be from same organisation")
        .And
        .HaveCount(1);
    }

    [Test]
    public void MustBeValidPreviousId_Valid_Succeeds()
    {
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase(prevId: Guid.NewGuid().ToString());

      validator.MustBeValidPreviousId();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidPreviousId_Null_Succeeds()
    {
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase(prevId: null);

      validator.MustBeValidPreviousId();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeValidPreviousId_NotGuid_ReturnsError()
    {
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase(prevId: "not a GUID");

      validator.MustBeValidPreviousId();
      var valres = validator.Validate(evidence);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Invalid PreviousId")
        .And
        .HaveCount(1);
    }

    [Test]
    public void PreviousMustBeForSameClaim_Same_Succeeds()
    {
      var prevId = Guid.NewGuid().ToString();
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase(prevId: prevId);
      var prevEvidence = Creator.GetEvidenceBase(claimId: evidence.ClaimId);
      _evidenceDatastore.Setup(x => x.ById(prevId)).Returns(prevEvidence);

      validator.PreviousMustBeForSameClaim();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void PreviousMustBeForSameClaim_Other_ReturnsError()
    {
      var prevId = Guid.NewGuid().ToString();
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase(prevId: prevId);
      var prevEvidence = Creator.GetEvidenceBase();
      _evidenceDatastore.Setup(x => x.ById(prevId)).Returns(prevEvidence);

      validator.PreviousMustBeForSameClaim();
      var valres = validator.Validate(evidence);

      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Previous evidence must be for same claim")
        .And
        .HaveCount(1);
    }

    [Test]
    public void PreviousMustNotBeInUse_NotInUse_Succeeds()
    {
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);
      var evidence = Creator.GetEvidenceBase(prevId: Guid.NewGuid().ToString());
      _evidenceDatastore.Setup(x => x.ByClaim(evidence.ClaimId)).Returns(new[] { new[] { Creator.GetEvidenceBase() } });

      validator.PreviousMustNotBeInUse();
      var valres = validator.Validate(evidence);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void PreviousMustNotBeInUse_InUse_ReturnsError()
    {
      var claimId = Guid.NewGuid().ToString();

      // first chain: ev1 <-- ev2
      var ev1 = Creator.GetEvidenceBase(claimId: claimId);
      var ev2 = Creator.GetEvidenceBase(claimId: claimId, prevId: ev1.Id);

      // second chain: evA <-- evB
      var evA = Creator.GetEvidenceBase(claimId: claimId);
      var evB = Creator.GetEvidenceBase(claimId: claimId, prevId: evA.Id);

      // evidence datastore returns both chains
      _evidenceDatastore.Setup(x => x.ByClaim(claimId))
        .Returns(new[] 
        {
          new[] { ev1, ev2 },
          new[] { evA, evB }
        });

      // create new evidence linked (previous) to ev1 ie 'fan out'
      var evidence = Creator.GetEvidenceBase(claimId: claimId, prevId: ev1.Id);
      var validator = new DummyEvidenceValidatorBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object, _logger.Object);


      validator.PreviousMustNotBeInUse();
      var valres = validator.Validate(evidence);


      valres.Errors.Should()
        .ContainSingle(x => x.ErrorMessage == "Previous evidence already in use")
        .And
        .HaveCount(1);
    }
  }
}
