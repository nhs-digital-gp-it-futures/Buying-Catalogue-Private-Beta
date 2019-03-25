using Dapper.Contrib.Extensions;
using System;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Keyword search history information
  /// </summary>
  [Table(nameof(Log))]
  public sealed class KeywordSearchHistory
  {
    /// <summary>
    /// UTC date and time at which keyword was searched
    /// Set by server when creating record
    /// SET ON SERVER
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Keyword
    /// </summary>
    public string Keyword { get; set; }
  }
}
