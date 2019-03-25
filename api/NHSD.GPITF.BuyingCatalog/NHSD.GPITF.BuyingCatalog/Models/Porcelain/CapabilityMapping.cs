using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Models.Porcelain
{
  /// <summary>
  /// A single Capability with its corresponding OptionalStandard
  /// </summary>
  public sealed class CapabilityMapping
  {
    /// <summary>
    /// <see cref="Capability"/>
    /// </summary>
    public Capabilities Capability { get; set; }

    /// <summary>
    /// A list of OptionalStandard
    /// </summary>
    public List<OptionalStandard> OptionalStandard { get; set; } = new List<OptionalStandard>();
  }
}
