using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.Porcelain
{
  public sealed class SolutionsExDatastore : DatastoreBase<SolutionEx>, ISolutionsExDatastore
  {
    private string ResourceBase { get; } = "/porcelain/SolutionsEx";

    private readonly ISolutionsDatastore _solutionDatastore;
    private readonly ITechnicalContactsDatastore _technicalContactDatastore;

    private readonly ICapabilitiesImplementedDatastore _claimedCapabilityDatastore;
    private readonly ICapabilitiesImplementedEvidenceDatastore _claimedCapabilityEvidenceDatastore;
    private readonly ICapabilitiesImplementedReviewsDatastore _claimedCapabilityReviewsDatastore;

    private readonly IStandardsApplicableDatastore _claimedStandardDatastore;
    private readonly IStandardsApplicableEvidenceDatastore _claimedStandardEvidenceDatastore;
    private readonly IStandardsApplicableReviewsDatastore _claimedStandardReviewsDatastore;

    public SolutionsExDatastore(
      IRestClientFactory crmConnectionFactory,
      ILogger<SolutionsExDatastore> logger,
      ISyncPolicyFactory policy,

      ISolutionsDatastore solutionDatastore,
      ITechnicalContactsDatastore technicalContactDatastore,

      ICapabilitiesImplementedDatastore claimedCapabilityDatastore,
      ICapabilitiesImplementedEvidenceDatastore claimedCapabilityEvidenceDatastore,
      ICapabilitiesImplementedReviewsDatastore claimedCapabilityReviewsDatastore,

      IStandardsApplicableDatastore claimedStandardDatastore,
      IStandardsApplicableEvidenceDatastore claimedStandardEvidenceDatastore,
      IStandardsApplicableReviewsDatastore claimedStandardReviewsDatastore,

      IConfiguration config) :
      base(crmConnectionFactory, logger, policy, config)
    {
      _solutionDatastore = solutionDatastore;
      _technicalContactDatastore = technicalContactDatastore;

      _claimedCapabilityDatastore = claimedCapabilityDatastore;
      _claimedCapabilityEvidenceDatastore = claimedCapabilityEvidenceDatastore;
      _claimedCapabilityReviewsDatastore = claimedCapabilityReviewsDatastore;

      _claimedStandardDatastore = claimedStandardDatastore;
      _claimedStandardEvidenceDatastore = claimedStandardEvidenceDatastore;
      _claimedStandardReviewsDatastore = claimedStandardReviewsDatastore;
    }

    public SolutionEx BySolution(string solutionId)
    {
      return GetInternal(() =>
      {
        var request = GetRequest($"{ResourceBase}/BySolution/{solutionId}");
        var retval = GetResponse<SolutionEx>(request);

        return retval;
      });
    }

    public void Update(SolutionEx solnEx)
    {
      GetInternal(() =>
      {
        var request = GetPutRequest($"{ResourceBase}/Update", solnEx);
        var resp = GetRawResponse(request);

        return 0;
      });
    }

    public IEnumerable<SolutionEx> ByOrganisation(string organisationId)
    {
      return GetInternal(() =>
      {
        var request = GetRequest($"{ResourceBase}/ByOrganisation/{organisationId}");
        var retval = GetResponse<IEnumerable<SolutionEx>>(request);

        return retval;
      });
    }
  }
}
