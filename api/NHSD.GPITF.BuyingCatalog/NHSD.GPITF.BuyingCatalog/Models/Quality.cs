using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  public abstract class Quality
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
    /// Name of Capability/Standard, as displayed to a user
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of Capability/Standard, as displayed to a user
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// URL with further information
    /// </summary>
    public string URL { get; set; }

    /// <summary>
    /// Category of Capability/Standard
    /// </summary>
    public string Type { get; set; }
  }
#pragma warning restore CS1591
}
