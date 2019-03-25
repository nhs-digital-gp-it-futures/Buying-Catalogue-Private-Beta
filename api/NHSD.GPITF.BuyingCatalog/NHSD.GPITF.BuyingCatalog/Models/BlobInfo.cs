using System;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Information about a folder or file in blob datastore,
  /// typically SharePoint
  /// </summary>
  public sealed class BlobInfo
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Unique identifier of parent/owner of this entity
    /// Will be null if this is the root entity
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// true if object is a folder
    /// </summary>
    public bool IsFolder { get; set; }

    /// <summary>
    /// size of file in bytes (zero for a folder)
    /// </summary>
    public long Length { get; set; }

    /// <summary>
    /// Externally accessible URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// UTC when last modified
    /// </summary>
    public DateTime TimeLastModified { get; set; }

    /// <summary>
    /// unique identifier of binary file in blob storage system
    /// (null for a folder)
    /// NOTE:  this may not be a GUID eg it may be a URL
    /// NOTE:  this is a GUID for SharePoint
    /// </summary>
    public string BlobId { get; set; }
  }
}
