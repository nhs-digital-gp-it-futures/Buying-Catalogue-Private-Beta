using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Link between a Capability and a Standard
  /// </summary>
  [Table(nameof(CapabilityStandard))]
  public sealed class CapabilityStandard
  {
    /// <summary>
    /// Unique identifier of Capability entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string CapabilityId { get; set; }

    /// <summary>
    /// Unique identifier of Standard entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string StandardId { get; set; }

    /// <summary>
    /// True if the Standard does not have to be supported in order to support the Capability
    /// </summary>
    public bool IsOptional { get; set; }
  }
}
