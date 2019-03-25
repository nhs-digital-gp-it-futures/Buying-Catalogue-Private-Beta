using Dapper.Contrib.Extensions;
using System;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// Log information from the application
  /// </summary>
  [Table(nameof(Log))]
  public sealed class Log
  {
    /// <summary>
    /// UTC date and time at which record created
    /// Set by server when creating record
    /// SET ON SERVER
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The log level (e.g. ERROR, DEBUG) or level ordinal (number):
    /// Fatal --> Highest level: important stuff down
    /// Error --> For example application crashes / exceptions.
    /// Warn --> Incorrect behavior but the application can continue
    /// Info --> Normal behavior like mail sent, user updated profile etc.
    /// Debug --> Executed queries, user authenticated, session expired
    /// Trace --> Begin method X, end method X etc
    /// </summary>
    public string Loglevel { get; set; }

    /// <summary>
    /// The call site (class name, method name and source information)
    /// </summary>
    public string Callsite { get; set; }

    /// <summary>
    /// Text of log message
    /// </summary>
    public string Message { get; set; }
  }
}
