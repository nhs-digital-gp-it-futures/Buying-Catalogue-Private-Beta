namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Status of a solution as it goes through various stages in its life cycle
  /// </summary>
  public enum SolutionStatus : int
  {
    /// <summary>
    /// <see cref="Solutions"/> has reached an end point
    /// </summary>
    Failed = -1,

    /// <summary>
    /// <see cref="Solutions"/> is being revised by <see cref="Organisations"/>
    /// </summary>
    Draft = 0,

    /// <summary>
    /// <see cref="Solutions"/> has been submitted by <see cref="Organisations"/>, for consideration
    /// by NHSD for inclusion into Buying Catalog
    /// </summary>
    Registered = 1,

    /// <summary>
    /// <see cref="Solutions"/> has been assessed by NHSD and has been accepted onto the
    /// Buying Catalog.  The <see cref="Solutions"/> will now have its <see cref="CapabilitiesImplemented"/>
    /// assessed by NHSD.
    /// </summary>
    CapabilitiesAssessment = 2,

    /// <summary>
    /// <see cref="CapabilitiesImplemented"/> have been verified by NHSD; and the <see cref="Solutions"/>
    /// will now have its <see cref="StandardsApplicable"/> assessed by NHSD.
    /// </summary>
    StandardsCompliance = 3,

    /// <summary>
    /// Solution has passed Standards Compliance and is awaiting final sign off (aka final approval).
    /// </summary>
    FinalApproval = 4,

    /// <summary>
    /// <see cref="StandardsApplicable"/> have been verified by NHSD; and the <see cref="Organisations"/>
    /// will now build its solution page for the Buying Catalog.
    /// </summary>
    SolutionPage = 5,

    /// <summary>
    /// The solution page has reviewed by NHS and the <see cref="Solutions"/> is now available for
    /// purchase on the Buying Catalog.
    /// Solution has reached an end point.
    /// </summary>
    Approved = 6
  }
}
