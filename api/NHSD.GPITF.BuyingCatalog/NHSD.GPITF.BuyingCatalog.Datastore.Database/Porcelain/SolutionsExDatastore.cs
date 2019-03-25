using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database.Porcelain
{
  public sealed class SolutionsExDatastore : DatastoreBase<SolutionEx>, ISolutionsExDatastore
  {
    private readonly ISolutionsDatastore _solutionDatastore;
    private readonly ITechnicalContactsDatastore _technicalContactDatastore;

    private readonly ICapabilitiesImplementedDatastore _claimedCapabilityDatastore;
    private readonly ICapabilitiesImplementedEvidenceDatastore _claimedCapabilityEvidenceDatastore;
    private readonly ICapabilitiesImplementedReviewsDatastore _claimedCapabilityReviewsDatastore;

    private readonly IStandardsApplicableDatastore _claimedStandardDatastore;
    private readonly IStandardsApplicableEvidenceDatastore _claimedStandardEvidenceDatastore;
    private readonly IStandardsApplicableReviewsDatastore _claimedStandardReviewsDatastore;

    public SolutionsExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<SolutionsExDatastore> logger,
      ISyncPolicyFactory policy,
      ISolutionsDatastore solutionDatastore,
      ITechnicalContactsDatastore technicalContactDatastore,

      ICapabilitiesImplementedDatastore claimedCapabilityDatastore,
      ICapabilitiesImplementedEvidenceDatastore claimedCapabilityEvidenceDatastore,
      ICapabilitiesImplementedReviewsDatastore claimedCapabilityReviewsDatastore,

      IStandardsApplicableDatastore claimedStandardDatastore,
      IStandardsApplicableEvidenceDatastore claimedStandardEvidenceDatastore,
      IStandardsApplicableReviewsDatastore claimedStandardReviewsDatastore
      ) :
      base(dbConnectionFactory, logger, policy)
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
        var retval = new SolutionEx
        {
          Solution = _solutionDatastore.ById(solutionId),
          TechnicalContact = _technicalContactDatastore.BySolution(solutionId).ToList(),
          ClaimedCapability = _claimedCapabilityDatastore.BySolution(solutionId).ToList(),
          ClaimedStandard = _claimedStandardDatastore.BySolution(solutionId).ToList()
        };

        // populate Evidence + Review
        retval.ClaimedCapabilityEvidence = retval.ClaimedCapability
          .SelectMany(cc => _claimedCapabilityEvidenceDatastore.ByClaim(cc.Id))
            .SelectMany(x => x)
            .ToList();
        retval.ClaimedCapabilityReview = retval.ClaimedCapabilityEvidence
          .SelectMany(cce => _claimedCapabilityReviewsDatastore.ByEvidence(cce.Id))
            .SelectMany(x => x)
            .ToList();
        retval.ClaimedStandardEvidence = retval.ClaimedStandard
          .SelectMany(cs => _claimedStandardEvidenceDatastore.ByClaim(cs.Id))
            .SelectMany(x => x)
            .ToList();
        retval.ClaimedStandardReview = retval.ClaimedStandardEvidence
          .SelectMany(cse => _claimedStandardReviewsDatastore.ByEvidence(cse.Id))
            .SelectMany(x => x)
            .ToList();

        return retval;
      });
    }

    public void Update(SolutionEx solnEx)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.Value.BeginTransaction())
        {
          // update Solution
          _dbConnection.Value.Update(solnEx.Solution, trans);

          #region ClaimedCapability
          // delete ClaimedCapabilities which will cascade delete Evidence + Reviews
          _claimedCapabilityDatastore
            .BySolution(solnEx.Solution.Id)
            .ToList()
            .ForEach(cc => _dbConnection.Value.Delete(cc, trans));

          // re-insert ClaimedCapabilities + Evidence + Reviews
          solnEx.ClaimedCapability.ForEach(cc => _dbConnection.Value.Insert(cc, trans));

          // re-insert each chain, starting at the root ie PreviousId==null
          GetInsertionTree(solnEx.ClaimedCapabilityEvidence).ForEach(cce => _dbConnection.Value.Insert(cce, trans));
          GetInsertionTree(solnEx.ClaimedCapabilityReview).ForEach(ccr => _dbConnection.Value.Insert(ccr, trans));
          #endregion

          #region ClaimedStandard
          // delete ClaimedStandards which will cascade delete Evidence + Reviews
          _claimedStandardDatastore
            .BySolution(solnEx.Solution.Id)
            .ToList()
            .ForEach(cs => _dbConnection.Value.Delete(cs, trans));

          // re-insert ClaimedStandards + Evidence + Reviews
          solnEx.ClaimedStandard.ForEach(cs => _dbConnection.Value.Insert(cs, trans));

          // re-insert each chain, starting at the root ie PreviousId==null
          GetInsertionTree(solnEx.ClaimedStandardEvidence).ForEach(cse => _dbConnection.Value.Insert(cse, trans));
          GetInsertionTree(solnEx.ClaimedStandardReview).ForEach(csr => _dbConnection.Value.Insert(csr, trans));
          #endregion

          #region TechnicalContacts
          // delete all TechnicalContact & re-insert
          _technicalContactDatastore
            .BySolution(solnEx.Solution.Id).ToList()
            .ForEach(tc => _dbConnection.Value.Delete(tc, trans));
          solnEx.TechnicalContact.ForEach(tc => _dbConnection.Value.Insert(tc, trans));
          #endregion

          trans.Commit();
        }

        return 0;
      });
    }

    public IEnumerable<SolutionEx> ByOrganisation(string organisationId)
    {
      return GetInternal(() =>
      {
        var solns = _solutionDatastore.ByOrganisation(organisationId);
        var retval = solns.Select(soln => BySolution(soln.Id));

        return retval;
      });
    }

    private static List<T> GetInsertionTree<T>(List<T> allNodes) where T : IHasPreviousId
    {
      var roots = GetRoots(allNodes);
      var tree = new List<T>(roots);

      var next = GetChildren(roots, allNodes);
      while (next.Any())
      {
        tree.AddRange(next);
        next = GetChildren(next, allNodes);
      }

      return tree;
    }

    private static List<T> GetRoots<T>(List<T> allNodes) where T : IHasPreviousId
    {
      return allNodes.Where(x => x.PreviousId == null).ToList();
    }

    private static List<T> GetChildren<T>(List<T> parents, List<T> allNodes) where T : IHasPreviousId
    {
      return parents.SelectMany(parent => allNodes.Where(x => x.PreviousId == parent.Id)).ToList();
    }
  }
}
