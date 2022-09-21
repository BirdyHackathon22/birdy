using Birdy.Web.Server.Services.Interfaces;
using Birdy.Web.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Birdy.Web.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BirdyWatchController : ControllerBase
    {
        private readonly ILogger<BirdyWatchController> _logger;
        private readonly IBirdyApiService birdyApiService;

        public BirdyWatchController(ILogger<BirdyWatchController> logger, IBirdyApiService birdyApiService)
        {
            _logger = logger;
            this.birdyApiService = birdyApiService;
        }

        [HttpGet]
        public async Task<IEnumerable<BirdyWatch>> Get()
        {
            return await birdyApiService.GetBirdyWatches();
        }
    }
}