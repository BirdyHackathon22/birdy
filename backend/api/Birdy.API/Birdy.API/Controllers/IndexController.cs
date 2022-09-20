using Microsoft.AspNetCore.Mvc;

namespace Birdy.API.Controllers
{
    [ApiController]
    [Route("")]
    public class IndexController : ControllerBase
    {
        private readonly ILogger<IndexController> _logger;

        public IndexController(ILogger<IndexController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Welcome")]
        public string Get()
        {
            return "Welcome to API";
        }
    }
}
