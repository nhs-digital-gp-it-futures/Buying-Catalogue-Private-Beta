using Microsoft.Extensions.Configuration;
using NHSD.GPITF.BuyingCatalog.Datastore.CRM.Interfaces;
using NUnit.Framework;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [SetUpFixture]
    public sealed class DatastoreBaseSetup
    {
      public static IRestClientFactory CrmConnectionFactory;

      [OneTimeSetUp]
      public void OneTimeSetUp()
      {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>();
        var config = builder.Build();

        CrmConnectionFactory = new RestClientFactory(config);
      }
    }
}
