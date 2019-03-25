using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// An agreement between an ‘organisation’ and NHS which allows a ‘solution’ to be purchased.  There may be more than one active or current ‘framework’ at any point in time  Note that a ‘framework’ has a link to zero or one previous ‘framework’  Generally, only interested in current ‘framework’  
  /// </summary>
  [Table(nameof(Frameworks))]
  public sealed class Frameworks
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
    /// Name of Framework, as displayed to a user
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of Framework, as displayed to a user
    /// </summary>
    public string Description { get; set; }
  }
}
