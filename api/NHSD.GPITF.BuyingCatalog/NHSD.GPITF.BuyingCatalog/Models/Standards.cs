using Dapper.Contrib.Extensions;
using NHSD.GPITF.BuyingCatalog.Interfaces;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A formal set of requirements eg  * ISO 9001
  /// Note that a ‘standard’ has a link to zero or one previous ‘standard’
  /// Generally, only interested in current ‘standard’
  /// </summary>
  [Table(nameof(Standards))]
  public sealed class Standards : Quality, IHasPreviousId
  {
    /// <summary>
    /// True if this standard applies to all solutions
    /// </summary>
    public bool IsOverarching { get; set; }
  }
}
