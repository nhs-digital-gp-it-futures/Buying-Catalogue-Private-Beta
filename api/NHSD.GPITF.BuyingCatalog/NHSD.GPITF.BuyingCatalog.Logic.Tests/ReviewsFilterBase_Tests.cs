using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic.Tests
{
  [TestFixture]
  public sealed class ReviewsFilterBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<IEvidenceDatastore<EvidenceBase>> _evidenceDatastore;
    private Mock<IClaimsDatastore<ClaimsBase>> _claimDatastore;
    private Mock<ISolutionsDatastore> _solutionDatastore;

    [SetUp]
    public void SetUp()
    {
      _context = new Mock<IHttpContextAccessor>();
      _evidenceDatastore = new Mock<IEvidenceDatastore<EvidenceBase>>();
      _claimDatastore = new Mock<IClaimsDatastore<ClaimsBase>>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new DummyReviewsFilterBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object));
    }

    [Test]
    public void Filter_Admin_ReturnsAll()
    {
      var filter = new DummyReviewsFilterBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object);
      var review = Creator.GetReviewsBase();

      var res = filter.FilterForAdmin(new[] { review });

      res.Should().BeEquivalentTo(review);
    }

    [Test]
    public void Filter_Buyer_ReturnsNull()
    {
      var filter = new DummyReviewsFilterBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object);
      var review = Creator.GetReviewsBase();

      var res = filter.FilterForBuyer(new[] { review });

      res.Should().BeNull();
    }

    [Test]
    public void Filter_SupplierOwn_ReturnsOwn()
    {
      var filter = new DummyReviewsFilterBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object);
      var orgId = Guid.NewGuid().ToString();
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var evidence = Creator.GetEvidenceBase(claimId: claim.Id);
      var review = Creator.GetReviewsBase(evidenceId: evidence.Id);
      var ctx = Creator.GetContext(orgId: orgId);
      _context.Setup(c => c.HttpContext).Returns(ctx);
      _evidenceDatastore.Setup(x => x.ById(review.EvidenceId)).Returns(evidence);
      _claimDatastore.Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      var res = filter.FilterForSupplier(new[] { review });

      res.Should().BeEquivalentTo(review);
    }

    [Test]
    public void Filter_SupplierOther_ReturnsNull()
    {
      var filter = new DummyReviewsFilterBase(_evidenceDatastore.Object, _claimDatastore.Object, _solutionDatastore.Object, _context.Object);
      var orgId = Guid.NewGuid().ToString();
      var soln = Creator.GetSolution(orgId: orgId);
      var claim = Creator.GetClaimsBase(solnId: soln.Id);
      var evidence = Creator.GetEvidenceBase(claimId: claim.Id);
      var review = Creator.GetReviewsBase(evidenceId: evidence.Id);
      var ctx = Creator.GetContext();
      _context.Setup(c => c.HttpContext).Returns(ctx);
      _evidenceDatastore.Setup(x => x.ById(review.EvidenceId)).Returns(evidence);
      _claimDatastore.Setup(x => x.ById(evidence.ClaimId)).Returns(claim);
      _solutionDatastore.Setup(x => x.ById(soln.Id)).Returns(soln);

      var res = filter.FilterForSupplier(new[] { review });

      res.Should().BeNull();
    }
  }
}
