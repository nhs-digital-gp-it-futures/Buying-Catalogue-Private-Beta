using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models.Porcelain
{
  /// <summary>
  /// A Standard and a flag associated with a Capability through a CapabilityMapping
  /// </summary>
  public sealed class OptionalStandard
  {
    /// <summary>
    /// Unique identifier of Standard
    /// </summary>
    [Required]
    public string StandardId { get; set; }

    /// <summary>
    /// True if the Standard does not have to be supported in order to support the Capability
    /// </summary>
    public bool IsOptional { get; set; }
  }
}
