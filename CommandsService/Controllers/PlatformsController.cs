using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{

    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {

        }
        [HttpGet]
        public ActionResult Ping()
        {
            Console.WriteLine("--> Ping Command Service");
            return Ok("Pong");
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine($"--> Inbound POST # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }

    }
}