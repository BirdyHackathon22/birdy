using Birdy.API.Models;
using Birdy.API.Models.Request;
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
            return await cosmosDbService.GetBirdyWatchesAsync();
        }

        [HttpGet("filter", Name = "GetBirdyWatchFilter")]
        public async Task<IEnumerable<BirdyWatch>> GetByFilter(string species, int score)
        {
            return await cosmosDbService.GetBirdyWatchesFilterAsync(species, score);
        }

        [HttpPost(Name = "CreateBirdyWatch")]
        public async Task<StatusCodeResult> Post(BirdyRequest request)
        {
            var birdyWatch = MapBirdyWatch(request);

            await cosmosDbService.CreateBirdyWatch(birdyWatch);

            return Ok();
        }

        private static BirdyWatch MapBirdyWatch(BirdyRequest request)
        {
            return new BirdyWatch(request.Label, new Location(long.Parse(request.Latitude), long.Parse(request.Longitude)))
            {
                ImageName = request.Path,
                Score = request.Score,
                BoundingBox = request.Box,
                DateSpotted = DateTime.Parse(request.Time),
                Device = request.Device
            };
        }
    }


}