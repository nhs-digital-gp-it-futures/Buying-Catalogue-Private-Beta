using Microsoft.SharePoint.Client.NetCore;

namespace NHSD.GPITF.BuyingCatalog
{
  internal sealed class SharePointIncluder
  {
#pragma warning disable CS0414
    // This is a workaround to force loading of Microsoft.SharePoint.Client.NetCore
    // assembly.  If this is not done, then Microsoft.SharePoint.Client.NetCore
    // will not be able to unmarshal json from SharePoint to strongly typed objects.
    // Go figure...
    private static readonly AlertType dummy = AlertType.Custom;
#pragma warning restore CS0414
  }
}
