using Dapper.Contrib.Extensions;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A list of potential competencies a ‘solution’ can perform or provide eg
  /// * Mobile working
  /// * Training
  /// * Prescribing
  /// * Installation
  /// Note that a ‘capability’ has a link to zero or one previous ‘capability’
  /// Generally, only interested in current ‘capability’
  /// </summary>
  [Table(nameof(Capabilities))]
  public sealed class Capabilities : Quality, IHasPreviousId
  {
  }
}
