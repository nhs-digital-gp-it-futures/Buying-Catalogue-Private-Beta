using Microsoft.Extensions.Configuration;
using System;

namespace NHSD.GPITF.BuyingCatalog.SystemTests
{
  internal static class Settings
  {
    public static string BASE_URL(IConfiguration config) => Environment.GetEnvironmentVariable("BASE_URL") ?? config["BASE_URL"] ?? "http://localhost:8000";
  }
}
