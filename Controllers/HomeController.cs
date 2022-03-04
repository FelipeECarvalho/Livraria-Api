using Microsoft.AspNetCore.Mvc;

namespace Livraria.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get()
        => Ok();

    }
}
