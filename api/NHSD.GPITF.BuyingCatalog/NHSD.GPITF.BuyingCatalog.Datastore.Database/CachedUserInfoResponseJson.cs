using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  [Table(nameof(CachedUserInfoResponseJson))]
  public sealed class CachedUserInfoResponseJson
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string Id { get; set; }

    /// <summary>
    /// Bearer token
    /// </summary>
    [Required]
    public string BearerToken { get; set; }

    /// <summary>
    /// JSON serialised CachedUserInfoResponse
    /// </summary>
    public string Data { get; set; }
  }
}
