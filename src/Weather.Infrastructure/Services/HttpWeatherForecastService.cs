using System;
using System.Net;
using Weather.Application.Abstractions;

namespace Weather.Infrastructure.Services
{
    public class HttpWeatherForecastService : IWeatherForecastService
    {
        public string GetForecast(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentException("Location must be provided", nameof(location));
            }

            var url = "https://wttr.in/" + location + "?format=3";
            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
