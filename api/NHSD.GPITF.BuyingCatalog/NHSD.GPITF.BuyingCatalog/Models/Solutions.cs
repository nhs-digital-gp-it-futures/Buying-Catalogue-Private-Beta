using Dapper.Contrib.Extensions;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A product and/or service provided by an ‘organisation’.
  /// Note that a ‘solution’ has a link to zero or one previous ‘solution’
  /// Generally, only interested in current ‘solution’
  /// Standard MS Dynamics CRM entity
  /// </summary>
  [Table(nameof(Solutions))]
  public sealed class Solutions : IHasPreviousId
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
    /// Unique identifier of organisation
    /// </summary>
    [Required]
    public string OrganisationId { get; set; }

    /// <summary>
    /// Version of solution
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Current status of this solution
    /// </summary>
    public SolutionStatus Status { get; set; } = SolutionStatus.Draft;

    /// <summary>
    /// Unique identifier of Contact who created this entity
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
    /// Unique identifier of Contact who last modified this entity
    /// Derived from calling context
    /// SET ON SERVER
    /// </summary>
    [Required]
    public string ModifiedById { get; set; }

    /// <summary>
    /// UTC date and time at which record last modified
    /// Set by server when creating record
    /// SET ON SERVER
    /// </summary>
    [Required]
    public DateTime ModifiedOn { get; set; }

    /// <summary>
    /// Name of solution, as displayed to a user
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of solution, as displayed to a user
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Serialised product page
    /// </summary>
    public string ProductPage { get; set; } = string.Empty;
  }
}
