using IdentityModel.Client;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IUserInfoResponseRetriever
  {
    Task<UserInfoResponse> GetAsync(string endPoint, string bearerToken);
  }
#pragma warning restore CS1591
}

