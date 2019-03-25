namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Status of a <see cref="CapabilitiesImplemented"/> as it goes through various stages in its life cycle
  /// </summary>
  public enum CapabilitiesImplementedStatus : int
  {
    /// <summary>
    /// <see cref="CapabilitiesImplemented"/> is being revised by <see cref="Organisations"/>
    /// </summary>
    Draft = 0,

    /// <summary>
    /// <see cref="CapabilitiesImplemented"/> has been submitted, for assessment by NHSD
    /// This is the starting point in the life cycle.
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// <see cref="CapabilitiesImplemented"/> is being revised by <see cref="Organisations"/>
    /// </summary>
    Remediation = 2,

    /// <summary>
    /// The Evidence has reviewed by NHSD and the <see cref="CapabilitiesImplemented"/> fulfills all
    /// of the requirements of the <see cref="Capabilities"/>
    /// This is a positive end point in the life cycle.
    /// </summary>
    Approved = 3,

    /// <summary>
    /// The Evidence has reviewed by NHSD and the <see cref="CapabilitiesImplemented"/> does NOT fulfill all
    /// of the requirements of the <see cref="Capabilities"/>
    /// This is a negative end point in the life cycle.
    /// An <see cref="Organisations"/> may NOT remove the <see cref="CapabilitiesImplemented"/>;
    /// they have to start a new submission ie create a new Solution.
    /// </summary>
    Rejected = 4
  }
}
