using System;
using Microsoft.Data.SqlClient;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using Weather.Application.Abstractions;

namespace Weather.Infrastructure.Services
{
    public class CsvWeatherDataImportService : IWeatherDataImportService
    {
        private readonly string _connectionString;

        public CsvWeatherDataImportService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Import(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
            {
                throw new ArgumentException("CSV path must be provided", nameof(csvPath));
            }

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("SQL connection string not configured");
            }

            using (var parser = new TextFieldParser(csvPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadLine();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    const string sql = "INSERT INTO WeatherData (StationName, DateTime, TempC, DewPointTempC, RelHum, PrecipAmountMm, WindDirDeg, WindSpdKmH, VisibilityKm, StnPressKPa, Hmdx, WindChill, Weather) VALUES (@StationName, @DateTime, @TempC, @DewPointTempC, @RelHum, @PrecipAmountMm, @WindDirDeg, @WindSpdKmH, @VisibilityKm, @StnPressKPa, @Hmdx, @WindChill, @Weather)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        while (!parser.EndOfData)
                        {
                            var fields = parser.ReadFields();
                            if (fields == null || fields.Length < 31)
                            {
                                continue;
                            }

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@StationName", fields[2]);
                            command.Parameters.AddWithValue("@DateTime", DateTime.Parse(fields[4], CultureInfo.InvariantCulture));
                            command.Parameters.AddWithValue("@TempC", ParseNullable(fields[10]));
                            command.Parameters.AddWithValue("@DewPointTempC", ParseNullable(fields[12]));
                            command.Parameters.AddWithValue("@RelHum", ParseNullableInt(fields[14]));
                            command.Parameters.AddWithValue("@PrecipAmountMm", ParseNullable(fields[16]));
                            command.Parameters.AddWithValue("@WindDirDeg", ParseNullableInt(fields[18]));
                            command.Parameters.AddWithValue("@WindSpdKmH", ParseNullable(fields[20]));
                            command.Parameters.AddWithValue("@VisibilityKm", ParseNullable(fields[22]));
                            command.Parameters.AddWithValue("@StnPressKPa", ParseNullable(fields[24]));
                            command.Parameters.AddWithValue("@Hmdx", ParseNullable(fields[26]));
                            command.Parameters.AddWithValue("@WindChill", ParseNullable(fields[28]));
                            command.Parameters.AddWithValue("@Weather", fields.Length > 30 ? fields[30] : (object)DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static object ParseNullable(string value)
        {
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return DBNull.Value;
        }

        private static object ParseNullableInt(string value)
        {
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return DBNull.Value;
        }
    }
}
