using RestSharp;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces
{
  public interface IRestClientFactory
  {
    IRestClient GetRestClient();
    AccessToken GetAccessToken();
  }
}
