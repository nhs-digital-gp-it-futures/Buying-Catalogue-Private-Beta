namespace NHSD.GPITF.BuyingCatalog.Models.Porcelain
{
  /// <summary>
  /// A SolutionEx and an indication of its relevance (Distance)
  /// </summary>
  public sealed class SearchResult
  {
    /// <summary>
    /// SolutionEx
    /// </summary>
    public SolutionEx SolutionEx { get; set; }

    /// <summary>
    /// How far away the SolutionEx is from ideal:
    ///   zero     => SolutionEx has exactly capabilities required
    ///   positive => SolutionEx has more capabilities than required
    ///   negative => SolutionEx has less capabilities than required
    /// </summary>
    public int Distance { get; set; }
  }
}
