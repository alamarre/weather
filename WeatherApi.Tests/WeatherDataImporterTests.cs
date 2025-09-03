using System;
using System.Data.SqlClient;
using System.IO;
using NUnit.Framework;
using WeatherApi.Services;

namespace WeatherApi.Tests
{
    public class WeatherDataImporterTests
    {
        [Test]
        public void Import_InsertsRows_WhenConnectionStringProvided()
        {
            var connStr = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connStr))
                Assert.Ignore("SQL connection string not set");

            var root = Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..");
            var sqlPath = Path.Combine(root, "sql", "CreateWeatherDataTable.sql");
            var csvPath = Path.Combine(root, "Data", "kitchener_2025_07.csv");

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("IF OBJECT_ID('WeatherData','U') IS NOT NULL DROP TABLE WeatherData", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                var createSql = File.ReadAllText(sqlPath);
                using (var cmd = new SqlCommand(createSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            WeatherDataImporter.Import(csvPath, connStr);

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM WeatherData", conn))
            {
                conn.Open();
                var count = (int)cmd.ExecuteScalar();
                Assert.Greater(count, 0);
            }
        }
    }
}
