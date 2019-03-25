using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.IO;

namespace NHSD.GPITF.BuyingCatalog
{
  internal static class WebHostBuilder
  {
    public static IWebHost BuildWebHost(string[] args)
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("hosting.json")
        .Build();

      return WebHost.CreateDefaultBuilder(args)
        .UseConfiguration(config)
        .UseKestrel()
        .ConfigureServices(services => services.AddAutofac())
        .UseStartup<Startup>()
        .ConfigureLogging(logging =>
        {
          logging.ClearProviders();
          logging.SetMinimumLevel(LogLevel.Trace);
        })
        .UseNLog()  // NLog: setup NLog for Dependency injection
        .Build();
    }
  }
}
