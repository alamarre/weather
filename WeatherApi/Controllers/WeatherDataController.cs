using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [RoutePrefix("api/weatherdata")]
    public class WeatherDataController : ApiController
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(DateTime? start = null, DateTime? end = null)
        {
            if (string.IsNullOrEmpty(_connectionString))
                return InternalServerError(new InvalidOperationException("SQL connection string not configured"));

            var list = new List<WeatherData>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT DateTime, TempC, DewPointTempC, RelHum, PrecipAmountMm, WindDirDeg, WindSpdKmH, VisibilityKm, StnPressKPa, Hmdx, WindChill, Weather FROM WeatherData WHERE (@start IS NULL OR DateTime >= @start) AND (@end IS NULL OR DateTime <= @end) ORDER BY DateTime", conn))
            {
                cmd.Parameters.AddWithValue("@start", (object)start ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@end", (object)end ?? DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new WeatherData
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
            return Ok(list);
        }
    }
}
