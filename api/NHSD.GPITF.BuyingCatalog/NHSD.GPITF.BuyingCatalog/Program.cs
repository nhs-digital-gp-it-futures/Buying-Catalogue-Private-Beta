using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace NHSD.GPITF.BuyingCatalog
{
#pragma warning disable CS1591
  public sealed class Program
  {
    public static void Main(string[] args)
    {
      // THIS IS THE MAGIC!
      // .NET Core assembly loading is confusing. Things that happen to be in your bin folder don't just suddenly
      // qualify with the assembly loader. If the assembly isn't specifically referenced by your app, you need to
      // tell .NET Core where to get it EVEN IF IT'S IN YOUR BIN FOLDER.
      // https://stackoverflow.com/questions/43918837/net-core-1-1-type-gettype-from-external-assembly-returns-null
      //
      // The documentation says that any .dll in the application base folder should work, but that doesn't seem
      // to be entirely true. You always have to set up additional handlers if you AREN'T referencing the plugin assembly.
      // https://github.com/dotnet/core-setup/blob/master/Documentation/design-docs/corehost.md
      //
      // To verify, try commenting this out and you'll see that the config system can't load the external plugin type.
      AssemblyLoadContext.Default.Resolving += (AssemblyLoadContext context, AssemblyName assembly) =>
      {
        // DISCLAIMER: NO PROMISES THIS IS SECURE. You may or may not want this strategy. It's up to
        // you to determine if allowing any assembly in the directory to be loaded is acceptable. This
        // is for demo purposes only.
        var assyPath = Assembly.GetExecutingAssembly().Location;
        var assyDir = Path.GetDirectoryName(assyPath);

        return context.LoadFromAssemblyPath(Path.Combine(assyDir, $"{assembly.Name}.dll"));
      };

      try
      {
        WebHostBuilder.BuildWebHost(args).Run();
      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }
  }
#pragma warning restore CS1591
}
