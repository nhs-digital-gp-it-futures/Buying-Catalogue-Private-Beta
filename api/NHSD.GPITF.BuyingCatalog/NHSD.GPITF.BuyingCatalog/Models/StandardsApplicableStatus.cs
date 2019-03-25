namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Status of a <see cref="StandardsApplicable"/> as it goes through various stages in its life cycle
  /// </summary>
  public enum StandardsApplicableStatus : int
  {
    /// <summary>
    /// Supplier has not added any evidence to support their claims
    /// Provided as a convenience to Suppliers
    /// This is the starting point in the life cycle.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// Supplier has started to add evidence to support their claims but 
    /// may still revise the evidence
    /// </summary>
    Draft = 1,

    /// <summary>
    /// <see cref="StandardsApplicable"/> has been submitted, for assessment by NHSD
    /// </summary>
    Submitted = 2,

    /// <summary>
    /// <see cref="StandardsApplicable"/> is being revised by <see cref="Organisations"/>
    /// </summary>
    Remediation = 3,

    /// <summary>
    /// The Evidence has reviewed by NHSD and the <see cref="StandardsApplicable"/> fulfills all
    /// of the requirements of the <see cref="Standards"/>
    /// This is a positive end point in the life cycle.
    /// </summary>
    Approved = 4,

    /// <summary>
    /// The Evidence has reviewed by NHSD and the <see cref="StandardsApplicable"/> fulfills all
    /// of the requirements of the <see cref="Standards"/>
    /// However, no installations of this Solution are in operation yet.
    /// This is a positive end point in the life cycle.
    /// </summary>
    ApprovedFirstOfType = 5,

    /// <summary>
    /// The Evidence has reviewed by NHSD and the <see cref="StandardsApplicable"/> fulfills enough
    /// of the requirements of the <see cref="Standards"/>
    /// This is a positive end point in the life cycle.
    /// </summary>
    ApprovedPartial = 6,

    /// <summary>
    /// The Evidence has reviewed by NHSD and the <see cref="StandardsApplicable"/> does NOT fulfill all
    /// of the requirements of the <see cref="Standards"/>
    /// This is a negative end point in the life cycle.
    /// An <see cref="Organisations"/> may remove the <see cref="StandardsApplicable"/>.
    /// </summary>
    Rejected = 7
  }
}
