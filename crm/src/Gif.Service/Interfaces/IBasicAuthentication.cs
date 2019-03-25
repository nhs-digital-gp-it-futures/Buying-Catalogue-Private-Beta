using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace Gif.Service.Interfaces
{
  public interface IBasicAuthentication
  {
    Task Authenticate(ValidatePrincipalContext context);
  }
}
