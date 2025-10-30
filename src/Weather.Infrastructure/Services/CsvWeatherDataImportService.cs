using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using Weather.Application.Abstractions;
using Weather.Infrastructure.Data;

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

            using (var reader = new StreamReader(csvPath))
            using (var connection = DbConnectionFactory.Create(_connectionString))
            {
                connection.Open();

                const string sql = "INSERT INTO WeatherData (StationName, DateTime, TempC, DewPointTempC, RelHum, PrecipAmountMm, WindDirDeg, WindSpdKmH, VisibilityKm, StnPressKPa, Hmdx, WindChill, Weather) VALUES (@StationName, @DateTime, @TempC, @DewPointTempC, @RelHum, @PrecipAmountMm, @WindDirDeg, @WindSpdKmH, @VisibilityKm, @StnPressKPa, @Hmdx, @WindChill, @Weather)";

                // skip header
                reader.ReadLine();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var fields = ParseCsvLine(line);
                        if (fields.Count < 31)
                        {
                            continue;
                        }

                        command.Parameters.Clear();
                        AddParameter(command, "@StationName", fields[2]);
                        AddParameter(command, "@DateTime", DateTime.Parse(fields[4], CultureInfo.InvariantCulture));
                        AddParameter(command, "@TempC", ParseNullable(fields[10]));
                        AddParameter(command, "@DewPointTempC", ParseNullable(fields[12]));
                        AddParameter(command, "@RelHum", ParseNullableInt(fields[14]));
                        AddParameter(command, "@PrecipAmountMm", ParseNullable(fields[16]));
                        AddParameter(command, "@WindDirDeg", ParseNullableInt(fields[18]));
                        AddParameter(command, "@WindSpdKmH", ParseNullable(fields[20]));
                        AddParameter(command, "@VisibilityKm", ParseNullable(fields[22]));
                        AddParameter(command, "@StnPressKPa", ParseNullable(fields[24]));
                        AddParameter(command, "@Hmdx", ParseNullable(fields[26]));
                        AddParameter(command, "@WindChill", ParseNullable(fields[28]));
                        AddParameter(command, "@Weather", fields.Count > 30 ? fields[30] : (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void AddParameter(IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        private static IReadOnlyList<string> ParseCsvLine(string line)
        {
            var values = new List<string>();
            if (line == null)
            {
                return values;
            }

            var builder = new StringBuilder();
            var inQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                var current = line[i];
                if (current == '\"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '\"')
                    {
                        builder.Append('\"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }

                    continue;
                }

                if (current == ',' && !inQuotes)
                {
                    values.Add(builder.ToString());
                    builder.Clear();
                    continue;
                }

                builder.Append(current);
            }

            values.Add(builder.ToString());

            return values;
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
