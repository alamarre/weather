using System;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

namespace WeatherApi.Services
{
    public static class WeatherDataImporter
    {
        public static void Import(string csvPath, string connectionString)
        {
            using (var parser = new TextFieldParser(csvPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadLine(); // skip header

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string sql = "INSERT INTO WeatherData (StationName, DateTime, TempC, DewPointTempC, RelHum, PrecipAmountMm, WindDirDeg, WindSpdKmH, VisibilityKm, StnPressKPa, Hmdx, WindChill, Weather) VALUES (@StationName, @DateTime, @TempC, @DewPointTempC, @RelHum, @PrecipAmountMm, @WindDirDeg, @WindSpdKmH, @VisibilityKm, @StnPressKPa, @Hmdx, @WindChill, @Weather)";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        while (!parser.EndOfData)
                        {
                            var fields = parser.ReadFields();
                            if (fields == null || fields.Length < 31) continue;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@StationName", fields[2]);
                            cmd.Parameters.AddWithValue("@DateTime", DateTime.Parse(fields[4], CultureInfo.InvariantCulture));
                            cmd.Parameters.AddWithValue("@TempC", ParseNullable(fields[10]));
                            cmd.Parameters.AddWithValue("@DewPointTempC", ParseNullable(fields[12]));
                            cmd.Parameters.AddWithValue("@RelHum", ParseNullableInt(fields[14]));
                            cmd.Parameters.AddWithValue("@PrecipAmountMm", ParseNullable(fields[16]));
                            cmd.Parameters.AddWithValue("@WindDirDeg", ParseNullableInt(fields[18]));
                            cmd.Parameters.AddWithValue("@WindSpdKmH", ParseNullable(fields[20]));
                            cmd.Parameters.AddWithValue("@VisibilityKm", ParseNullable(fields[22]));
                            cmd.Parameters.AddWithValue("@StnPressKPa", ParseNullable(fields[24]));
                            cmd.Parameters.AddWithValue("@Hmdx", ParseNullable(fields[26]));
                            cmd.Parameters.AddWithValue("@WindChill", ParseNullable(fields[28]));
                            cmd.Parameters.AddWithValue("@Weather", fields.Length > 30 ? fields[30] : null);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static object ParseNullable(string value)
        {
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return DBNull.Value;
        }

        private static object ParseNullableInt(string value)
        {
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return DBNull.Value;
        }
    }
}
