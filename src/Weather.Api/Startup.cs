using System;
using System.Collections.Generic;
using System.Web.Http;
using Owin;
using Weather.Api.Controllers;
using Weather.Api.Infrastructure;
using Weather.Application.Abstractions;
using Weather.Application.Services;
using Weather.Infrastructure.Data;
using Weather.Infrastructure.Services;

namespace Weather.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            Configure(config);
            app.UseWebApi(config);
        }

        public static void Configure(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

            var repository = new SqlWeatherDataRepository(connectionString);
            IWeatherDataService weatherDataService = new WeatherDataService(repository);
            IWeatherForecastService forecastService = new HttpWeatherForecastService();
            IWeatherDataImportService importService = new CsvWeatherDataImportService(connectionString);

            var registrations = new Dictionary<Type, Func<object>>
            {
                { typeof(IWeatherDataService), () => weatherDataService },
                { typeof(IWeatherForecastService), () => forecastService },
                { typeof(IWeatherDataImportService), () => importService },
                { typeof(WeatherDataController), () => new WeatherDataController(weatherDataService) },
                { typeof(WeatherController), () => new WeatherController(forecastService) },
                { typeof(PingController), () => new PingController() }
            };

            config.DependencyResolver = new SimpleDependencyResolver(registrations);
        }
    }
}
