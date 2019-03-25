using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using Polly;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Search.Porcelain
{
  public sealed class SearchDatastore : ISearchDatastore
  {
    private readonly ILogger<SearchDatastore> _logger;
    private readonly ISyncPolicy _policy;
    private readonly IFrameworksDatastore _frameworkDatastore;
    private readonly ISolutionsDatastore _solutionDatastore;
    private readonly ICapabilitiesDatastore _capabilityDatastore;
    private readonly ICapabilitiesImplementedDatastore _claimedCapabilityDatastore;
    private readonly ISolutionsExDatastore _solutionExDatastore;

    public SearchDatastore(
      ILogger<SearchDatastore> logger,
      ISyncPolicyFactory policy,
      IFrameworksDatastore frameworkDatastore,
      ISolutionsDatastore solutionDatastore,
      ICapabilitiesDatastore capabilityDatastore,
      ICapabilitiesImplementedDatastore claimedCapabilityDatastore,
      ISolutionsExDatastore solutionExDatastore)
    {
      _logger = logger;
      _policy = policy.Build(_logger);
      _frameworkDatastore = frameworkDatastore;
      _solutionDatastore = solutionDatastore;
      _capabilityDatastore = capabilityDatastore;
      _claimedCapabilityDatastore = claimedCapabilityDatastore;
      _solutionExDatastore = solutionExDatastore;
    }

    public IEnumerable<SearchResult> ByKeyword(string keyword)
    {
      _logger.LogInformation($"{keyword}");

      return _policy.Execute(() =>
      {
        // get all Frameworks
        var allFrameworks = _frameworkDatastore.GetAll();

        // get all Solutions via frameworks
        var allSolns = allFrameworks
          .SelectMany(fw => _solutionDatastore.ByFramework(fw.Id));

        // get all Capabilities via frameworks
        var allCapsFrameworks = allFrameworks
          .SelectMany(fw => _capabilityDatastore.ByFramework(fw.Id));

        // get all Capabilities with keyword
        var allCapsKeywordIds = allCapsFrameworks
          .Where(cap =>
            cap.Name.ToLowerInvariant().Contains(keyword.ToLowerInvariant()) ||
            cap.Description.ToLowerInvariant().Contains(keyword.ToLowerInvariant()))
          .Select(cap => cap.Id);

        // get all unique Solutions with at least one ClaimedCapability with keyword
        var allSolnsCapsKeyword = allSolns
          .Where(soln => _claimedCapabilityDatastore
            .BySolution(soln.Id)
            .Select(cc => cc.CapabilityId)
            .Intersect(allCapsKeywordIds)
            .Any())
          .Distinct();

        var searchResults = allSolnsCapsKeyword.Select(soln =>
          new SearchResult
          {
            SolutionEx = _solutionExDatastore.BySolution(soln.Id),
            Distance = _claimedCapabilityDatastore.BySolution(soln.Id).Count() - allCapsKeywordIds.Count()
          });

        return searchResults;
      });
    }
  }
}
