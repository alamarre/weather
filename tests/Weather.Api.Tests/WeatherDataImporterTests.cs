using System;
using System.Data;
using System.IO;
using NUnit.Framework;
using Weather.Infrastructure.Data;
using Weather.Infrastructure.Services;

namespace Weather.Api.Tests
{
    public class WeatherDataImporterTests
    {
        [Test]
        public void Import_InsertsRows_WhenConnectionStringProvided()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"weather-test-{Guid.NewGuid():N}.db");
            var connStr = $"sqlite:Data Source={dbPath};Version=3;";
            Environment.SetEnvironmentVariable("SQL_CONNECTION_STRING", connStr);

            var root = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", ".."));
            var csvPath = Path.Combine(root, "Data", "kitchener_2025_07.csv");

            using (var conn = DbConnectionFactory.Create(connStr))
            {
                conn.Open();
                ExecuteNonQuery(conn, "DROP TABLE IF EXISTS WeatherData");
                ExecuteNonQuery(conn, @"CREATE TABLE WeatherData (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StationName TEXT NOT NULL,
    DateTime TEXT NOT NULL,
    TempC REAL NULL,
    DewPointTempC REAL NULL,
    RelHum INTEGER NULL,
    PrecipAmountMm REAL NULL,
    WindDirDeg INTEGER NULL,
    WindSpdKmH REAL NULL,
    VisibilityKm REAL NULL,
    StnPressKPa REAL NULL,
    Hmdx REAL NULL,
    WindChill REAL NULL,
    Weather TEXT NULL
)");
            }

            var importer = new CsvWeatherDataImportService(connStr);
            importer.Import(csvPath);

            using (var conn = DbConnectionFactory.Create(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(*) FROM WeatherData";
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                Assert.Greater(count, 0);
            }

            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        private static void ExecuteNonQuery(IDbConnection connection, string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }
}
