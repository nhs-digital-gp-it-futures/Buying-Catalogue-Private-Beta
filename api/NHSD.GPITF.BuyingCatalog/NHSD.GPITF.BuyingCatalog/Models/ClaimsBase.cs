using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  public abstract class ClaimsBase
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string Id { get; set; }

    /// <summary>
    /// Unique identifier of solution
    /// </summary>
    [Required]
    public string SolutionId { get; set; }

    /// <summary>
    /// Unique identifier of supplier Contact who is responsible for this claim
    /// </summary>
    public string OwnerId { get; set; }

    /// <summary>
    /// Unique identifier of Capability or Standard
    /// </summary>
    [Computed]
    public abstract string QualityId { get; }

    /// <summary>
    /// UTC date and time at which record was originally created
    /// Set by server when first creating record
    /// SET ON SERVER
    /// </summary>
    [Required]
    public DateTime OriginalDate { get; set; }
  }
#pragma warning restore CS1591
}
