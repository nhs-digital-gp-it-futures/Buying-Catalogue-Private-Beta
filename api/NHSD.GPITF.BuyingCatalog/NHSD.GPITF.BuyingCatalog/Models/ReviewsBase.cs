using Dapper.Contrib.Extensions;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  public abstract class ReviewsBase : IHasPreviousId
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string Id { get; set; }

    /// <summary>
    /// Unique identifier of previous version of entity
    /// </summary>
    public string PreviousId { get; set; }

    /// <summary>
    /// Unique identifier of associated Evidence
    /// </summary>
    [Required]
    public string EvidenceId { get; set; }

    /// <summary>
    /// Unique identifier of Contact who created record
    /// Derived from calling context
    /// SET ON SERVER
    /// </summary>
    [Required]
    public string CreatedById { get; set; }

    /// <summary>
    /// UTC date and time at which record created
    /// Set by server when creating record
    /// SET ON SERVER
    /// </summary>
    [Required]
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// UTC date and time at which record was originally created
    /// Set by server when first creating record
    /// SET ON SERVER
    /// </summary>
    [Required]
    public DateTime OriginalDate { get; set; }

    /// <summary>
    /// Serialised message data
    /// </summary>
    public string Message { get; set; } = string.Empty;
  }
#pragma warning restore CS1591
}
