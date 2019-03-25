using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class DbConnectionFactory : IDbConnectionFactory
  {
    private readonly IConfiguration _config;

    public DbConnectionFactory(IConfiguration config)
    {
      _config = config;
    }

    public IDbConnection Get()
    {
      var connection = Settings.DATASTORE_CONNECTION(_config);
      var connType = Settings.DATASTORE_CONNECTIONTYPE(_config, connection);
      var dbType = Enum.Parse<DataAccessProviderTypes>(connType);

      // HACK:  workaround for PostgreSql which puts table+column names in lower case
      if (dbType == DataAccessProviderTypes.PostgreSql)
      {
        SqlMapperExtensions.TableNameMapper = LowerCaseTableNameMapper;
      }

      var dbFact = DbProviderFactoryUtils.GetDbProviderFactory(dbType);
      var dbConn = dbFact.CreateConnection();

      dbConn.ConnectionString = Settings.DATASTORE_CONNECTIONSTRING(_config, connection).Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory);
      dbConn.Open();

      return dbConn;
    }

    private static string LowerCaseTableNameMapper(Type type)
    {
      var tableattr = type.GetCustomAttributes<TableAttribute>(false).SingleOrDefault();
      var name = string.Empty;

      if (tableattr != null)
      {
        name = tableattr.Name.ToLowerInvariant();
      }

      return name;
    }
  }
}
