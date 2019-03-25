using Dapper.Contrib.Extensions;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Initially, a 'message' or response to 'evidence' which supports a claim to a ‘standard’.
  /// Thereafter, this will be a response to a previous message/response.
  /// </summary>
  [Table(nameof(StandardsApplicableReviews))]
  public sealed class StandardsApplicableReviews : ReviewsBase
  {
  }
}
