using System.Net;
using System.Web.Http;

namespace WeatherApi.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeatherController : ApiController
    {
        [HttpGet]
        [Route("{location}")]
        public IHttpActionResult Get(string location)
        {
            var url = "https://wttr.in/" + location + "?format=3";
            using (var client = new WebClient())
            {
                var result = client.DownloadString(url);
                return Ok(result);
            }
        }
    }
}
