using Microsoft.SharePoint.Client.NetCore;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public interface IAuthenticationManager
  {
    /// <summary>
    /// Returns an app only ClientContext object
    /// </summary>
    /// <param name="siteUrl">Site for which the ClientContext object will be instantiated</param>
    /// <param name="appId">Application ID which is requesting the ClientContext object</param>
    /// <param name="appSecret">Application secret of the Application which is requesting the ClientContext object</param>
    /// <returns>ClientContext to be used by CSOM code</returns>
    ClientContext GetAppOnlyAuthenticatedContext(string siteUrl, string appId, string appSecret);
  }
}
