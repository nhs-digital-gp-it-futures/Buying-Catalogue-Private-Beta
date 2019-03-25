namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IUserInfoResponseCache
  {
    // [bearerToken]-->[UserInfoResponse]
    bool TryGetValue(string bearerToken, out string jsonCachedResponse);

    void SafeAdd(string bearerToken, string jsonCachedResponse);
    void Remove(string bearerToken);
  }
#pragma warning restore CS1591
}

