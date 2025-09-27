using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Weather.Domain.Entities;
using Weather.Domain.Repositories;

namespace Weather.Infrastructure.Data
{
    public class SqlWeatherDataRepository : IWeatherDataRepository
    {
        private const string Query = @"SELECT DateTime, TempC, DewPointTempC, RelHum, PrecipAmountMm, WindDirDeg, WindSpdKmH, VisibilityKm, StnPressKPa, Hmdx, WindChill, Weather
                                      FROM WeatherData
                                      WHERE (@start IS NULL OR DateTime >= @start)
                                        AND (@end IS NULL OR DateTime <= @end)
                                      ORDER BY DateTime";

        private readonly string _connectionString;

        public SqlWeatherDataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<WeatherData> GetWeatherData(DateTime? start, DateTime? end)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("SQL connection string not configured");
            }

            var results = new List<WeatherData>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(Query, connection))
            {
                command.Parameters.AddWithValue("@start", (object)start ?? DBNull.Value);
                command.Parameters.AddWithValue("@end", (object)end ?? DBNull.Value);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new WeatherData
                        {
                            DateTime = reader.GetDateTime(0),
                            TempC = reader.IsDBNull(1) ? (double?)null : reader.GetDouble(1),
                            DewPointTempC = reader.IsDBNull(2) ? (double?)null : reader.GetDouble(2),
                            RelHum = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            PrecipAmountMm = reader.IsDBNull(4) ? (double?)null : reader.GetDouble(4),
                            WindDirDeg = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            WindSpdKmH = reader.IsDBNull(6) ? (double?)null : reader.GetDouble(6),
                            VisibilityKm = reader.IsDBNull(7) ? (double?)null : reader.GetDouble(7),
                            StnPressKPa = reader.IsDBNull(8) ? (double?)null : reader.GetDouble(8),
                            Hmdx = reader.IsDBNull(9) ? (double?)null : reader.GetDouble(9),
                            WindChill = reader.IsDBNull(10) ? (double?)null : reader.GetDouble(10),
                            Weather = reader.IsDBNull(11) ? null : reader.GetString(11)
                        });
                    }
                }
            }

            return results;
        }
    }
}
