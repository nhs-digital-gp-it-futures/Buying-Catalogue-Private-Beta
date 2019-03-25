using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Models.Porcelain
{
  /// <summary>
  /// An Extended Solution with its corresponding Technical Contacts, ClaimedCapability, ClaimedStandard et al
  /// </summary>
  public sealed class SolutionEx
  {
    /// <summary>
    /// Solution
    /// </summary>
    public Solutions Solution { get; set; }

    /// <summary>
    /// A list of ClaimedCapability
    /// </summary>
    public List<CapabilitiesImplemented> ClaimedCapability { get; set; } = new List<CapabilitiesImplemented>();

    /// <summary>
    /// A list of ClaimedCapabilityEvidence
    /// </summary>
    public List<CapabilitiesImplementedEvidence> ClaimedCapabilityEvidence { get; set; } = new List<CapabilitiesImplementedEvidence>();

    /// <summary>
    /// A list of ClaimedCapabilityReview
    /// </summary>
    public List<CapabilitiesImplementedReviews> ClaimedCapabilityReview { get; set; } = new List<CapabilitiesImplementedReviews>();

    /// <summary>
    /// A list of ClaimedStandard
    /// </summary>
    public List<StandardsApplicable> ClaimedStandard { get; set; } = new List<StandardsApplicable>();

    /// <summary>
    /// A list of ClaimedStandardEvidence
    /// </summary>
    public List<StandardsApplicableEvidence> ClaimedStandardEvidence { get; set; } = new List<StandardsApplicableEvidence>();

    /// <summary>
    /// A list of ClaimedStandardReview
    /// </summary>
    public List<StandardsApplicableReviews> ClaimedStandardReview { get; set; } = new List<StandardsApplicableReviews>();

    /// <summary>
    /// A list of TechnicalContact
    /// </summary>
    public List<TechnicalContacts> TechnicalContact { get; set; } = new List<TechnicalContacts>();
  }
}
