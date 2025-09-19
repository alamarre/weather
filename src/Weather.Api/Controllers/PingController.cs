using System.Web.Http;

namespace Weather.Api.Controllers
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
