namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IDatastoreCache
  {
    bool TryGetValue(string path, out string jsonCachedResponse);
    void SafeAdd(string path, string jsonCachedResponse);
  }
#pragma warning restore CS1591
}

