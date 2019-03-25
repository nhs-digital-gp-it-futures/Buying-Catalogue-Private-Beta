using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  [Table(nameof(FrameworkStandard))]
  public sealed class FrameworkStandard
  {
    [Required]
    [ExplicitKey]
    public string FrameworkId { get; set; }

    [Required]
    [ExplicitKey]
    public string StandardId { get; set; }
  }
#pragma warning restore CS1591
}
