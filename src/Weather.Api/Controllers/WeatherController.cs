using System.Web.Http;
using Weather.Application.Abstractions;

namespace Weather.Api.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeatherController : ApiController
    {
        private readonly IWeatherForecastService _forecastService;

        public WeatherController(IWeatherForecastService forecastService)
        {
            _forecastService = forecastService;
        }

        [HttpGet]
        [Route("{location}")]
        public IHttpActionResult Get(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return BadRequest("Location must be provided.");
            }

            var result = _forecastService.GetForecast(location);
            return Ok(result);
        }
    }
}
