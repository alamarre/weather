using System;
using System.Data;
using System.Data.SqlClient;
using Mono.Data.Sqlite;

namespace Weather.Infrastructure.Data
{
    public static class DbConnectionFactory
    {
        private const string SqlitePrefix = "sqlite:";

        public static IDbConnection Create(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("SQL connection string not configured");
            }

            if (connectionString.StartsWith(SqlitePrefix, StringComparison.OrdinalIgnoreCase))
            {
                var sqliteConnectionString = connectionString.Substring(SqlitePrefix.Length);
                if (string.IsNullOrWhiteSpace(sqliteConnectionString))
                {
                    throw new InvalidOperationException("SQLite connection string must be provided after the 'sqlite:' prefix");
                }

                return new SqliteConnection(sqliteConnectionString);
            }

            return new SqlConnection(connectionString);
        }
    }
}
