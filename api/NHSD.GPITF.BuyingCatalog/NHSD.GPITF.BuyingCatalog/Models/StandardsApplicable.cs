using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A ‘Standard’ which a ‘Solution’ asserts that it provides.
  /// This is then assessed by NHS to verify the ‘Solution’ complies with the ‘Standard’ it has claimed.
  /// </summary>
  [Table(nameof(StandardsApplicable))]
  public sealed class StandardsApplicable : ClaimsBase
  {
    /// <summary>
    /// Unique identifier of standard
    /// </summary>
    [Required]
    public string StandardId { get; set; }

    /// <summary>
    /// Current status of this ClaimedStandard
    /// </summary>
    public StandardsApplicableStatus Status { get; set; } = StandardsApplicableStatus.Submitted;

    /// <summary>
    /// Unique identifier of Standard
    /// </summary>
    [Computed]
    public override string QualityId => StandardId;

    /// <summary>
    /// UTC date and time at which Status is changed to Submitted
    /// Set by server when updating record
    /// SET ON SERVER
    /// </summary>
    [Required]
    public DateTime SubmittedOn { get; set; }

    /// <summary>
    /// UTC date and time at which record was assigned to a TechnicalContact
    /// Set by server when updating record
    /// SET ON SERVER
    /// </summary>
    [Required]
    public DateTime AssignedOn { get; set; }
  }
}
