using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
#pragma warning disable CS1591
  [Table(nameof(FrameworkSolution))]
  public sealed class FrameworkSolution
  {
    [Required]
    [ExplicitKey]
    public string FrameworkId { get; set; }

    [Required]
    [ExplicitKey]
    public string SolutionId { get; set; }
  }
#pragma warning restore CS1591
}
