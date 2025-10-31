using Microsoft.AspNetCore.Mvc;

namespace Weather.Api.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController
    (
        ILogger<PingController>  logger  
    ) : ControllerBase
{

    [HttpGet(Name = "Ping")]
    public IResult Get()
    {
        return TypedResults.Ok("Pong");
    }
}