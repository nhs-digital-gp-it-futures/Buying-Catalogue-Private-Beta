using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Search.Porcelain;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NUnit.Framework;
using Polly;
using System.Collections.Generic;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using System;
using NHSD.GPITF.BuyingCatalog.Tests;

namespace NHSD.GPITF.BuyingCatalog.Search.Tests
{
  [TestFixture]
  public sealed class SearchDatastore_Tests
  {
    private Mock<ILogger<SearchDatastore>> _logger;
    private Mock<ISyncPolicyFactory> _policyFact;
    private Mock<ISyncPolicy> _policy;
    private Mock<IFrameworksDatastore> _frameworkDatastore;
    private Mock<ISolutionsDatastore> _solutionDatastore;
    private Mock<ICapabilitiesDatastore> _capabilityDatastore;
    private Mock<ICapabilitiesImplementedDatastore> _claimedCapabilityDatastore;
    private Mock<ISolutionsExDatastore> _solutionExDatastore;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger<SearchDatastore>>();
      _policyFact = new Mock<ISyncPolicyFactory>();
      _policy = new Mock<ISyncPolicy>();
      _frameworkDatastore = new Mock<IFrameworksDatastore>();
      _solutionDatastore = new Mock<ISolutionsDatastore>();
      _capabilityDatastore = new Mock<ICapabilitiesDatastore>();
      _claimedCapabilityDatastore = new Mock<ICapabilitiesImplementedDatastore>();
      _solutionExDatastore = new Mock<ISolutionsExDatastore>();

      _policyFact.Setup(x => x.Build(_logger.Object)).Returns(_policy.Object);
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object));
    }

    [TestCase("Document Manager", "doc")]
    [TestCase("Document Manager", "manage")]
    [TestCase("Patient Collaboration", "collab")]
    [TestCase("Patient Collaboration", "ient")]
    public void ByKeyword_KeywordInCapabilityName_ReturnsSolution(
      string capabilityName,
      string keyword)
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln });

      var capability = Creator.GetCapability(name: capabilityName);
      _capabilityDatastore.Setup(x => x.ById(capability.Id)).Returns(capability);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { capability });

      var claimedCapability = Creator.GetCapabilitiesImplemented(solnId: soln.Id, claimId: capability.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln.Id)).Returns(new[] { claimedCapability });

      var solnEx = Creator.GetSolutionEx(soln: soln);
      _solutionExDatastore.Setup(x => x.BySolution(soln.Id)).Returns(solnEx);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword(keyword);

      var res = results.Should().ContainSingle();
      res.Which.SolutionEx.Should().BeEquivalentTo(solnEx);
      res.Which.Distance.Should().Be(0);
    }

    [TestCase("Does Really Kool document management", "doc")]
    [TestCase("Does Really Kool document management", "manage")]
    [TestCase("Does Really Kool patient collaboration", "collab")]
    [TestCase("Does Really Kool patient collaboration", "ient")]
    public void ByKeyword_KeywordInCapabilityDescription_ReturnsSolution(
      string capabilityDescription,
      string keyword)
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln });

      var capability = Creator.GetCapability(description: capabilityDescription);
      _capabilityDatastore.Setup(x => x.ById(capability.Id)).Returns(capability);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { capability });

      var claimedCapability = Creator.GetCapabilitiesImplemented(solnId: soln.Id, claimId: capability.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln.Id)).Returns(new[] { claimedCapability });

      var solnEx = Creator.GetSolutionEx(soln: soln);
      _solutionExDatastore.Setup(x => x.BySolution(soln.Id)).Returns(solnEx);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword(keyword);

      var res = results.Should().ContainSingle();

      res.Which.SolutionEx.Should().BeEquivalentTo(solnEx);
      res.Which.Distance.Should().Be(0);
    }

    [TestCase("Document Manager", "docs")]
    [TestCase("Document Manager", "manages")]
    [TestCase("Patient Collaboration", "collaborates")]
    [TestCase("Patient Collaboration", "sentient")]
    public void ByKeyword_KeywordNotInCapabilityName_ReturnsNone(
      string capabilityName,
      string keyword)
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln });

      var capability = Creator.GetCapability(name: capabilityName);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { capability });

      var claimedCapability = Creator.GetCapabilitiesImplemented(solnId: soln.Id, claimId: capability.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln.Id)).Returns(new[] { claimedCapability });

      var solnEx = Creator.GetSolutionEx(soln: soln);
      _solutionExDatastore.Setup(x => x.BySolution(soln.Id)).Returns(solnEx);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword(keyword);

      results.Should().BeEmpty();
    }

    [TestCase("Does Really Kool document management", "docs")]
    [TestCase("Does Really Kool document management", "manages")]
    [TestCase("Does Really Kool patient collaboration", "collaborates")]
    [TestCase("Does Really Kool patient collaboration", "sentient")]
    public void ByKeyword_KeywordNotInCapabilityDescription_ReturnsNone(
      string capabilityDescription,
      string keyword)
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln });

      var capability = Creator.GetCapability(description: capabilityDescription);
      _capabilityDatastore.Setup(x => x.ById(capability.Id)).Returns(capability);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { capability });

      var claimedCapability = Creator.GetCapabilitiesImplemented(solnId: soln.Id, claimId: capability.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln.Id)).Returns(new[] { claimedCapability });

      var solnEx = Creator.GetSolutionEx(soln: soln);
      _solutionExDatastore.Setup(x => x.BySolution(soln.Id)).Returns(solnEx);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword(keyword);

      results.Should().BeEmpty();
    }

    [TestCase("Does Really Kool document management", "doc")]
    [TestCase("Does Really Kool document management", "manage")]
    [TestCase("Does Really Kool patient collaboration", "collab")]
    [TestCase("Does Really Kool patient collaboration", "ient")]
    public void ByKeyword_SolutionMultiCapability_ReturnsSolution(
      string capabilityDescription,
      string keyword)
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln1 = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln1 });

      var cap1 = Creator.GetCapability(description: capabilityDescription);
      var cap2 = Creator.GetCapability();
      _capabilityDatastore.Setup(x => x.ById(cap1.Id)).Returns(cap1);
      _capabilityDatastore.Setup(x => x.ById(cap2.Id)).Returns(cap2);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { cap1, cap2 });

      var claimedCap11 = Creator.GetCapabilitiesImplemented(solnId: soln1.Id, claimId: cap1.Id);
      var claimedCap12 = Creator.GetCapabilitiesImplemented(solnId: soln1.Id, claimId: cap2.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(new[] { claimedCap11, claimedCap12 });

      var solnEx1 = Creator.GetSolutionEx(soln: soln1);
      _solutionExDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(solnEx1);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword(keyword);

      var res = results.Should().ContainSingle();
      res.Which.SolutionEx.Should().BeEquivalentTo(solnEx1);
      res.Which.Distance.Should().Be(1);
    }

    [TestCase("Does Really Kool document management", "doc")]
    [TestCase("Does Really Kool document management", "manage")]
    [TestCase("Does Really Kool patient collaboration", "collab")]
    [TestCase("Does Really Kool patient collaboration", "ient")]
    public void ByKeyword_KeywordMultiCapability_ReturnsSolution(
      string capabilityDescription,
      string keyword)
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln1 = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln1 });

      var cap1 = Creator.GetCapability(description: capabilityDescription);
      var cap2 = Creator.GetCapability(description: capabilityDescription);
      _capabilityDatastore.Setup(x => x.ById(cap1.Id)).Returns(cap1);
      _capabilityDatastore.Setup(x => x.ById(cap2.Id)).Returns(cap2);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { cap1, cap2 });

      var claimedCap11 = Creator.GetCapabilitiesImplemented(solnId: soln1.Id, claimId: cap1.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(new[] { claimedCap11 });

      var solnEx1 = Creator.GetSolutionEx(soln: soln1);
      _solutionExDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(solnEx1);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword(keyword);

      var res = results.Should().ContainSingle();
      res.Which.SolutionEx.Should().BeEquivalentTo(solnEx1);
      res.Which.Distance.Should().Be(-1);
    }

    [TestCase("Does Really Kool document management", "doc")]
    [TestCase("Does Really Kool document management", "manage")]
    [TestCase("Does Really Kool patient collaboration", "collab")]
    [TestCase("Does Really Kool patient collaboration", "ient")]
    public void ByKeyword_MultiCapability_ReturnsSolution(
      string capabilityDescription,
      string keyword)
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln1 = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln1 });

      var cap1 = Creator.GetCapability(description: capabilityDescription);
      var cap2 = Creator.GetCapability(description: capabilityDescription);
      _capabilityDatastore.Setup(x => x.ById(cap1.Id)).Returns(cap1);
      _capabilityDatastore.Setup(x => x.ById(cap2.Id)).Returns(cap2);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { cap1, cap2 });

      var claimedCap11 = Creator.GetCapabilitiesImplemented(solnId: soln1.Id, claimId: cap1.Id);
      var claimedCap12 = Creator.GetCapabilitiesImplemented(solnId: soln1.Id, claimId: cap2.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(new[] { claimedCap11, claimedCap12 });

      var solnEx1 = Creator.GetSolutionEx(soln: soln1);
      _solutionExDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(solnEx1);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword(keyword);

      var res = results.Should().ContainSingle();
      res.Which.SolutionEx.Should().BeEquivalentTo(solnEx1);
      res.Which.Distance.Should().Be(0);
    }

    [Test]
    public void ByKeyword_MultiCapabilityMultiSolutions_ReturnsSolutions()
    {
      var framework = Creator.GetFramework();
      _frameworkDatastore.Setup(x => x.GetAll()).Returns(new[] { framework });

      var soln1 = Creator.GetSolution();
      var soln2 = Creator.GetSolution();
      _solutionDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { soln1, soln2 });

      var cap1 = Creator.GetCapability(description: "capabilityDescription");
      var cap2 = Creator.GetCapability(description: "capabilityDescription");
      _capabilityDatastore.Setup(x => x.ById(cap1.Id)).Returns(cap1);
      _capabilityDatastore.Setup(x => x.ById(cap2.Id)).Returns(cap2);
      _capabilityDatastore.Setup(x => x.ByFramework(framework.Id)).Returns(new[] { cap1, cap2 });

      var claimedCap1_s1 = Creator.GetCapabilitiesImplemented(solnId: soln1.Id, claimId: cap1.Id);
      var claimedCap2_s1 = Creator.GetCapabilitiesImplemented(solnId: soln1.Id, claimId: cap2.Id);
      var claimedCap1_s2 = Creator.GetCapabilitiesImplemented(solnId: soln2.Id, claimId: cap1.Id);
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(new[] { claimedCap1_s1, claimedCap2_s1 });
      _claimedCapabilityDatastore.Setup(x => x.BySolution(soln2.Id)).Returns(new[] { claimedCap1_s2 });

      var solnEx1 = Creator.GetSolutionEx(soln: soln1);
      var solnEx2 = Creator.GetSolutionEx(soln: soln2);
      _solutionExDatastore.Setup(x => x.BySolution(soln1.Id)).Returns(solnEx1);
      _solutionExDatastore.Setup(x => x.BySolution(soln2.Id)).Returns(solnEx2);

      IEnumerable<SearchResult> results = null;
      _policy.Setup(x => x.Execute(It.IsAny<Func<IEnumerable<SearchResult>>>()))
        .Callback((Func<IEnumerable<SearchResult>> action) => results = action())
        .Returns(results);

      var search = new SearchDatastore(
        _logger.Object,
        _policyFact.Object,
        _frameworkDatastore.Object,
        _solutionDatastore.Object,
        _capabilityDatastore.Object,
        _claimedCapabilityDatastore.Object,
        _solutionExDatastore.Object);

      search.ByKeyword("descr");

      results.Should().HaveCount(2);
      results.Should()
        .ContainSingle(x => x.SolutionEx == solnEx1)
        .Which
        .Distance.Should().Be(0);
      results.Should()
        .ContainSingle(x => x.SolutionEx == solnEx2)
        .Which
        .Distance.Should().Be(-1);
    }
  }
}
