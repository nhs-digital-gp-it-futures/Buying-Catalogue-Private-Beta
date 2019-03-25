namespace NHSD.GPITF.BuyingCatalog
{
  /// <summary>
  /// User roles within Buying Catalog
  /// </summary>
  public static class Roles
  {
    /// <summary>
    /// An entity purchasing a Solution
    /// </summary>
    public const string Buyer = "Buyer";

    /// <summary>
    /// An entity providing a Solution (software and/or services)
    /// </summary>
    public const string Supplier = "Supplier";

    /// <summary>
    /// An entity managing all data within Buying Catalog, typically NHS Digital
    /// </summary>
    public const string Admin = "Admin";
  }
}
