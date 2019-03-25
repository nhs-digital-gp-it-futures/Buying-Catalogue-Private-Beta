namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// How may times a keyword has appeared in a search
  /// </summary>
  public sealed class KeywordCount
  {
    /// <summary>
    /// Keyword
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// Number of times this keyword has occurred
    /// </summary>
    public int Count { get; set; }
  }
}
