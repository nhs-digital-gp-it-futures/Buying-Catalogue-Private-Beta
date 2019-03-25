using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  [Table(nameof(CapabilityFramework))]
  public sealed class CapabilityFramework
  {
    [Required]
    [ExplicitKey]
    public string CapabilityId { get; set; }

    [Required]
    [ExplicitKey]
    public string FrameworkId { get; set; }
  }
#pragma warning restore CS1591
}
