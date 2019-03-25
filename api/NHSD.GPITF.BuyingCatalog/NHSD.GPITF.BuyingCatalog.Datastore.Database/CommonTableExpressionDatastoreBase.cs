using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using System;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public abstract class CommonTableExpressionDatastoreBase<T> : DatastoreBase<T>
  {
    public CommonTableExpressionDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<CommonTableExpressionDatastoreBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    /// <summary>
    /// Removes 'recursive' keyword from Common Table Expression when
    /// database is MS SQL Server as this database does not support this
    /// keyword.
    /// 
    /// 'Recursive' keyword is definitely required for MySQL and PostgreSql
    /// but is ignored by SQLite.
    /// </summary>
    /// <param name="sql">generic SQL statement containing keyword 'recursive'</param>
    /// <returns>SQL statement for specific database</returns>
    protected string AmendCommonTableExpression(string sql)
    {
      if (!sql.Contains("recursive"))
      {
        throw new ArgumentException("SQL Common Table Expression modified - cannot find 'recursive'");
      }

      var dbType = _dbConnection.Value.GetType().ToString();
      switch (dbType)
      {
        case "Microsoft.Data.Sqlite.SqliteConnection":
        case "MySql.Data.MySqlClient.MySqlConnection":
        case "Npgsql.NpgsqlConnection":
          return sql;

        case "System.Data.SqlClient.SqlConnection":
          return sql.Replace("recursive", "");

        default:
          throw new ArgumentOutOfRangeException($"Untested database: {dbType}");
      }
    }
  }
}
