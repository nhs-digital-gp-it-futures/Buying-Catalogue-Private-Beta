using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace NHSD.GPITF.BuyingCatalog
{
#pragma warning disable CS1591
  public static class Settings
  {
    public static string OIDC_ISSUER_URL(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_ISSUER_URL") ?? config["Jwt:Authority"];
    public static string OIDC_AUDIENCE(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_AUDIENCE") ?? config["Jwt:Audience"];
    public static string OIDC_USERINFO_URL(IConfiguration config) => Environment.GetEnvironmentVariable("OIDC_USERINFO_URL") ?? config["Jwt:UserInfo"];

    public static bool USE_CRM(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("USE_CRM") ?? config["UseCRM"] ?? false.ToString());

    public static string LOG_CONNECTIONSTRING(IConfiguration config) => Environment.GetEnvironmentVariable("LOG_CONNECTIONSTRING") ?? config["Log:ConnectionString"];
    public static bool LOG_CRM(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("LOG_CRM") ?? config["Log:CRM"] ?? false.ToString());
    public static bool LOG_SHAREPOINT(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("LOG_SHAREPOINT") ?? config["Log:SharePoint"] ?? false.ToString());
    public static bool LOG_BEARERAUTH(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("LOG_BEARERAUTH") ?? config["Log:BearerAuth"] ?? false.ToString());

    public static string DATASTORE_CONNECTION(IConfiguration config) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTION") ?? config["RepositoryDatabase:Connection"];
    public static string DATASTORE_CONNECTIONTYPE(IConfiguration config, string connection) => Environment.GetEnvironmentVariable("DATASTORE_CONNECTIONTYPE") ?? config[$"RepositoryDatabase:{connection}:Type"];
    public static string DATASTORE_CONNECTIONSTRING(IConfiguration config, string connection) => (Environment.GetEnvironmentVariable("DATASTORE_CONNECTIONSTRING") ?? config[$"RepositoryDatabase:{connection}:ConnectionString"]);

    public static string CRM_APIURI(IConfiguration config) => Environment.GetEnvironmentVariable("CRM_APIURI") ?? config["CRM:ApiUri"];
    public static string CRM_ACCESSTOKENURI(IConfiguration config) => Environment.GetEnvironmentVariable("CRM_ACCESSTOKENURI") ?? config["CRM:AccessTokenUri"];
    public static string CRM_CLIENTID(IConfiguration config) => Environment.GetEnvironmentVariable("CRM_CLIENTID") ?? config["CRM:ClientId"];
    public static string CRM_CLIENTSECRET(IConfiguration config) => Environment.GetEnvironmentVariable("CRM_CLIENTSECRET") ?? config["CRM:ClientSecret"];
    public static int CRM_CACHE_EXPIRY_MINS(IConfiguration config) => int.Parse(Environment.GetEnvironmentVariable("CRM_CACHE_EXPIRY_MINS") ?? config["CRM:CacheExpiryMins"] ?? (7*24*60).ToString(CultureInfo.InvariantCulture));

    public static string SHAREPOINT_BASEURL(IConfiguration config) => Environment.GetEnvironmentVariable("SHAREPOINT_BASEURL") ?? config["SharePoint:BaseUrl"];
    public static string SHAREPOINT_ORGANISATIONSRELATIVEURL(IConfiguration config) => Environment.GetEnvironmentVariable("SHAREPOINT_ORGANISATIONSRELATIVEURL") ?? config["SharePoint:OrganisationsRelativeUrl"];
    public static string SHAREPOINT_CLIENT_ID(IConfiguration config) => Environment.GetEnvironmentVariable("SHAREPOINT_CLIENT_ID") ?? config["SharePoint:ClientId"];
    public static string SHAREPOINT_CLIENT_SECRET(IConfiguration config) => Environment.GetEnvironmentVariable("SHAREPOINT_CLIENT_SECRET") ?? config["SharePoint:ClientSecret"];
    public static string SHAREPOINT_LOGIN(IConfiguration config) => Environment.GetEnvironmentVariable("SHAREPOINT_LOGIN") ?? config["SharePoint:Login"];
    public static string SHAREPOINT_PASSWORD(IConfiguration config) => Environment.GetEnvironmentVariable("SHAREPOINT_PASSWORD") ?? config["SharePoint:Password"];
    public static string SHAREPOINT_PROVIDER_ENV(IConfiguration config) => Environment.GetEnvironmentVariable("SHAREPOINT_PROVIDER_ENV");
    public static bool SHAREPOINT_PROVIDER_FAKE(IConfiguration config) => SHAREPOINT_PROVIDER_ENV(config) == "test";

    public static string CACHE_HOST(IConfiguration config) => Environment.GetEnvironmentVariable("CACHE_HOST") ?? config["Cache:Host"] ?? "localhost";
  }
#pragma warning restore CS1591
}
