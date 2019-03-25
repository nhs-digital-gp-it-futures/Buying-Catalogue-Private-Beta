using Dapper.Contrib.Extensions;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A piece of 'evidence' which supports a claim to a ‘standard’.
  /// This is then assessed by NHS to verify the ‘solution’ complies with the ‘standard’ it has claimed.
  /// </summary>
  [Table(nameof(StandardsApplicableEvidence))]
  public sealed class StandardsApplicableEvidence : EvidenceBase
  {
  }
}
