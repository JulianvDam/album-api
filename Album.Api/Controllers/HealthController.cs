using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult CheckHealth()
        {
            try {
                return Ok("Application is healthy");
            } catch {
                return Ok("ERROR: Application is not healthy");
            }
        }
    }
}
