using Dapper.Contrib.Extensions;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A piece of 'evidence' which supports a claim to a ‘capability’.
  /// This is then assessed by NHS to verify the ‘solution’ complies with the ‘capability’ it has claimed.
  /// </summary>
  [Table(nameof(CapabilitiesImplementedEvidence))]
  public sealed class CapabilitiesImplementedEvidence : EvidenceBase
  {
  }
}
