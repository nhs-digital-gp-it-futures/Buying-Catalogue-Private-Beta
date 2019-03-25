using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Mapping between a claimed capability or standard and it's corresponding
  /// folders and files in blob datastore
  /// </summary>
  public sealed class ClaimBlobInfoMap
  {
    /// <summary>
    /// Unique identifier of claimed capability or standard
    /// </summary>
    public string ClaimId { get; set; }

    /// <summary>
    /// List of folders and files associated with this claimed capability or standard
    /// </summary>
    public IEnumerable<BlobInfo> BlobInfos { get; set; }
  }
}
