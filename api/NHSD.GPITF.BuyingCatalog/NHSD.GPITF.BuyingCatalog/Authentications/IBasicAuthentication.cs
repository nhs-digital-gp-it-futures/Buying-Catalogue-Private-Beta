using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace NHSD.GPITF.BuyingCatalog.Authentications
{
#pragma warning disable CS1591
  public interface IBasicAuthentication
  {
    Task Authenticate(ValidatePrincipalContext context);
  }
#pragma warning restore CS1591
}
