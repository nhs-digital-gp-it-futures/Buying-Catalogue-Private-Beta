using Microsoft.Extensions.Configuration;
using System;

namespace Gif.Service
{
  internal static class Settings
  {
    public static string LOG_CONNECTIONSTRING(IConfiguration config) => Environment.GetEnvironmentVariable("LOG_CONNECTIONSTRING") ?? config["Log:ConnectionString"];
    public static bool LOG_CRM(IConfiguration config) => bool.Parse(Environment.GetEnvironmentVariable("LOG_CRM") ?? config["Log:CRM"] ?? false.ToString());

    public static string GIF_AUTHORITY_URI(IConfiguration config) => Environment.GetEnvironmentVariable("GIF_AUTHORITY_URI") ?? config["GIF:Authority_Uri"] ?? "http://localhost:5001";
    public static string GIF_CRM_AUTHORITY(IConfiguration config) => Environment.GetEnvironmentVariable("GIF_CRM_AUTHORITY") ?? config["CrmAuthority"];
    public static string GIF_CRM_URL(IConfiguration config) => Environment.GetEnvironmentVariable("GIF_CRM_URL") ?? config["CrmUrl"];
    public static string GIF_AZURE_CLIENT_ID(IConfiguration config) => Environment.GetEnvironmentVariable("GIF_AZURE_CLIENT_ID") ?? config["AzureClientId"];
    public static string GIF_ENCRYPTED_CLIENT_SECRET(IConfiguration config) => Environment.GetEnvironmentVariable("GIF_ENCRYPTED_CLIENT_SECRET") ?? config["EncryptedClientSecret"];

    public static string CRM_CLIENTID(IConfiguration config) => Environment.GetEnvironmentVariable("CRM_CLIENTID") ?? config["CRM:ClientId"];
    public static string CRM_CLIENTSECRET(IConfiguration config) => Environment.GetEnvironmentVariable("CRM_CLIENTSECRET") ?? config["CRM:ClientSecret"];
  }
}
