using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.IO;

namespace Gif.Service
{
  public class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        var configuration = new ConfigurationBuilder()
          .AddJsonFile("hosting.json")
          .AddEnvironmentVariables()
          .Build();

        var host = new WebHostBuilder()
          .UseKestrel()
          .UseContentRoot(Directory.GetCurrentDirectory())
          .UseConfiguration(configuration)
          .UseStartup<Startup>()
          .ConfigureLogging(logging =>
          {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
          })
          .UseNLog()  // NLog: setup NLog for Dependency injection
          .Build();

        host.Run();

      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }
  }
}
