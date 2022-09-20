using Birdy.API.Models;
using Birdy.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Birdy.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BirdyWatchController : ControllerBase
    {
        private readonly ILogger<BirdyWatchController> _logger;
        private readonly ICosmosDbService cosmosDbService;

        public BirdyWatchController(ILogger<BirdyWatchController> logger, ICosmosDbService cosmosDbService)
        {
            _logger = logger;
            this.cosmosDbService = cosmosDbService;
        }

        [HttpGet(Name = "GetBirdyWatch")]
        public async Task<IEnumerable<BirdyWatch>> Get()
        {
            return await cosmosDbService.GetAnimalsAsync();
        }
    }
}