using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic.Porcelain
{
  public sealed class SolutionsExLogic : LogicBase, ISolutionsExLogic
  {
    private readonly ISolutionsModifier _solutionsModifier;

    private readonly ICapabilitiesImplementedModifier _capabilitiesImplementedModifier;
    private readonly IStandardsApplicableModifier _standardsApplicableModifier;

    private readonly ICapabilitiesImplementedEvidenceModifier _capabilitiesImplementedEvidenceModifier;
    private readonly IStandardsApplicableEvidenceModifier _standardsApplicableEvidenceModifier;

    private readonly ICapabilitiesImplementedReviewsModifier _capabilitiesImplementedReviewsModifier;
    private readonly IStandardsApplicableReviewsModifier _standardsApplicableReviewsModifier;

    private readonly ISolutionsExDatastore _datastore;
    private readonly ISolutionsExValidator _validator;
    private readonly ISolutionsExFilter _filter;
    private readonly IContactsDatastore _contacts;
    private readonly IEvidenceBlobStoreLogic _evidenceBlobStoreLogic;

    public SolutionsExLogic(
      ISolutionsModifier solutionsModifier,

      ICapabilitiesImplementedModifier capabilitiesImplementedModifier,
      IStandardsApplicableModifier standardsApplicableModifier,

      ICapabilitiesImplementedEvidenceModifier capabilitiesImplementedEvidenceModifier,
      IStandardsApplicableEvidenceModifier standardsApplicableEvidenceModifier,

      ICapabilitiesImplementedReviewsModifier capabilitiesImplementedReviewsModifier,
      IStandardsApplicableReviewsModifier standardsApplicableReviewsModifier,

      ISolutionsExDatastore datastore,
      IHttpContextAccessor context,
      ISolutionsExValidator validator,
      ISolutionsExFilter filter,
      IContactsDatastore contacts,
      IEvidenceBlobStoreLogic evidenceBlobStoreLogic) :
      base(context)
    {
      _solutionsModifier = solutionsModifier;

      _capabilitiesImplementedModifier = capabilitiesImplementedModifier;
      _standardsApplicableModifier = standardsApplicableModifier;

      _capabilitiesImplementedReviewsModifier = capabilitiesImplementedReviewsModifier;
      _standardsApplicableReviewsModifier = standardsApplicableReviewsModifier;

      _capabilitiesImplementedEvidenceModifier = capabilitiesImplementedEvidenceModifier;
      _standardsApplicableEvidenceModifier = standardsApplicableEvidenceModifier;

      _datastore = datastore;
      _validator = validator;
      _filter = filter;
      _contacts = contacts;
      _evidenceBlobStoreLogic = evidenceBlobStoreLogic;
    }

    public SolutionEx BySolution(string solutionId)
    {
      return _filter.Filter(new[] { _datastore.BySolution(solutionId) }).SingleOrDefault();
    }

    public void Update(SolutionEx solnEx)
    {
      _validator.ValidateAndThrowEx(solnEx, ruleSet: nameof(ISolutionsExLogic.Update));

      _solutionsModifier.ForUpdate(solnEx.Solution);

      solnEx.ClaimedCapability.ForEach(claim => _capabilitiesImplementedModifier.ForUpdate(claim));
      solnEx.ClaimedStandard.ForEach(claim => _standardsApplicableModifier.ForUpdate(claim));

      solnEx.ClaimedCapabilityEvidence.ForEach(evidence => _capabilitiesImplementedEvidenceModifier.ForUpdate(evidence));
      solnEx.ClaimedStandardEvidence.ForEach(evidence => _standardsApplicableEvidenceModifier.ForUpdate(evidence));

      solnEx.ClaimedCapabilityReview.ForEach(review =>_capabilitiesImplementedReviewsModifier.ForUpdate(review));
      solnEx.ClaimedStandardReview.ForEach(review =>_standardsApplicableReviewsModifier.ForUpdate(review));

      _datastore.Update(solnEx);

      // create SharePoint folder structure
      if (solnEx.Solution.Status == SolutionStatus.Registered)
      {
        _evidenceBlobStoreLogic.PrepareForSolution(solnEx.Solution.Id);
      }
    }

    public IEnumerable<SolutionEx> ByOrganisation(string organisationId)
    {
      return _filter.Filter(_datastore.ByOrganisation(organisationId));
    }
  }
}
