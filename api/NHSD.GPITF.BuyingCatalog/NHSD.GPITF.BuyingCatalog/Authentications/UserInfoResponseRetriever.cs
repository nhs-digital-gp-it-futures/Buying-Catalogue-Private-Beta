using IdentityModel.Client;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.Authentications
{
#pragma warning disable CS1591
  public sealed class UserInfoResponseRetriever : IUserInfoResponseRetriever
  {
    public Task<UserInfoResponse> GetAsync(string endPoint, string bearerToken)
    {
      var userInfoClient = new UserInfoClient(endPoint);
      var response = userInfoClient.GetAsync(bearerToken);
      return response;
    }
  }
#pragma warning restore CS1591
}
