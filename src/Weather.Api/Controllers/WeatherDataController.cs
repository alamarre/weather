using System;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> Get(DateTime? start = null, DateTime? end = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var data = await _weatherDataService.GetWeatherData(start, end, cancellationToken);
                return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
