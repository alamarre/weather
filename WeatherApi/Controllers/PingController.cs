using System.Web.Http;

namespace WeatherApi.Controllers
{
    public class PingController : ApiController
    {
        [HttpGet]
        [Route("ping")]
        public string Get()
        {
            return "pong";
        }
    }
}
