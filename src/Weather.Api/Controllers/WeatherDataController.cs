using System;
using System.Web.Http;
using Weather.Application.Abstractions;

namespace Weather.Api.Controllers
{
    [RoutePrefix("api/weatherdata")]
    public class WeatherDataController : ApiController
    {
        private readonly IWeatherDataService _weatherDataService;

        public WeatherDataController(IWeatherDataService weatherDataService)
        {
            _weatherDataService = weatherDataService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(DateTime? start = null, DateTime? end = null)
        {
            try
            {
                var data = _weatherDataService.GetWeatherData(start, end);
                return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
