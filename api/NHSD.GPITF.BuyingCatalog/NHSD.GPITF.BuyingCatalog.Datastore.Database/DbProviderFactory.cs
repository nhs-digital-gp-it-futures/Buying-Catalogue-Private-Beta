using System;
using System.Data.Common;
using System.Data.SqlClient;
using Westwind.Utilities;
using Westwind.Utilities.Properties;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  // stolen from:
  //
  //    https://weblog.west-wind.com/posts/2017/Nov/27/Working-around-the-lack-of-dynamic-DbProviderFactory-loading-in-NET-Core

  internal enum DataAccessProviderTypes
  {
    SqlServer,
    SqLite,
    MySql,
    PostgreSql,

#if NETFULL
    OleDb,
    SqlServerCompact
#endif
  }

  internal static class DbProviderFactoryUtils
  {
    public static DbProviderFactory GetDbProviderFactory(DataAccessProviderTypes type)
    {
      if (type == DataAccessProviderTypes.SqlServer)
        return SqlClientFactory.Instance; // this library has a ref to SqlClient so this works

      if (type == DataAccessProviderTypes.SqLite)
      {
#if NETFULL
        return GetDbProviderFactory("System.Data.SQLite.SQLiteFactory", "System.Data.SQLite");
#else
        return GetDbProviderFactory("Microsoft.Data.Sqlite.SqliteFactory", "Microsoft.Data.Sqlite");
#endif
      }
      if (type == DataAccessProviderTypes.MySql)
        return GetDbProviderFactory("MySql.Data.MySqlClient.MySqlClientFactory", "MySql.Data");
      if (type == DataAccessProviderTypes.PostgreSql)
        return GetDbProviderFactory("Npgsql.NpgsqlFactory", "Npgsql");
#if NETFULL
    if (type == DataAccessProviderTypes.OleDb)
        return System.Data.OleDb.OleDbFactory.Instance;
    if (type == DataAccessProviderTypes.SqlServerCompact)
        return DbProviderFactories.GetFactory("System.Data.SqlServerCe.4.0");
#endif

      throw new NotSupportedException(string.Format(Resources.UnsupportedProviderFactory, type.ToString()));
    }

    public static DbProviderFactory GetDbProviderFactory(string providerName)
    {
#if NETFULL
    return DbProviderFactories.GetFactory(providerName);
#else
      var providername = providerName.ToLower();

      if (providerName == "system.data.sqlclient")
        return GetDbProviderFactory(DataAccessProviderTypes.SqlServer);
      if (providerName == "system.data.sqlite" || providerName == "microsoft.data.sqlite")
        return GetDbProviderFactory(DataAccessProviderTypes.SqLite);
      if (providerName == "mysql.data.mysqlclient" || providername == "mysql.data")
        return GetDbProviderFactory(DataAccessProviderTypes.MySql);
      if (providerName == "npgsql")
        return GetDbProviderFactory(DataAccessProviderTypes.PostgreSql);

      throw new NotSupportedException(string.Format(Resources.UnsupportedProviderFactory, providerName));
#endif
    }

    public static DbProviderFactory GetDbProviderFactory(string dbProviderFactoryTypename, string assemblyName)
    {
      var instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypename, "Instance");
      if (instance == null)
      {
        var a = ReflectionUtils.LoadAssembly(assemblyName);
        if (a != null)
          instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypename, "Instance");
      }

      if (instance == null)
        throw new InvalidOperationException(string.Format(Resources.UnableToRetrieveDbProviderFactoryForm, dbProviderFactoryTypename));

      return instance as DbProviderFactory;
    }
  }
}
