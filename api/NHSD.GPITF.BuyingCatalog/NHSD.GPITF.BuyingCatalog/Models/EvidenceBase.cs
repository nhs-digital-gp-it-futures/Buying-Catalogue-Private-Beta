using Dapper.Contrib.Extensions;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  public abstract class EvidenceBase : IHasPreviousId
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
    /// Unique identifier of Claim
    /// </summary>
    [Required]
    public string ClaimId { get; set; }

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
    /// Serialised evidence data
    /// </summary>
    public string Evidence { get; set; } = string.Empty;

    /// <summary>
    /// true if supplier has requested to do a 'live demo'
    /// instead of submitting a file
    /// </summary>
    public bool HasRequestedLiveDemo { get; set; }

    /// <summary>
    /// unique identifier of binary file in blob storage system
    /// NOTE:  this may not be a GUID eg it may be a URL
    /// NOTE:  this is a GUID for SharePoint
    /// </summary>
    public string BlobId { get; set; }
  }
#pragma warning restore CS1591
}
