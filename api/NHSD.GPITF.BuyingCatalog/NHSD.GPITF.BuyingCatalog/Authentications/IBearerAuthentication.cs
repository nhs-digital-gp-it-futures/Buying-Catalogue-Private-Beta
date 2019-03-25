using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.Authentications
{
#pragma warning disable CS1591
  public interface IBearerAuthentication
  {
    Task Authenticate(TokenValidatedContext context);
  }
#pragma warning restore CS1591
}

