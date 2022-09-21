using Birdy.API.Models;
using Birdy.API.Models.Request;
using Birdy.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Birdy.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly ILogger<VoteController> _logger;
        private readonly ICosmosDbService cosmosDbService;

        public VoteController(ILogger<VoteController> logger, ICosmosDbService cosmosDbService)
        {
            _logger = logger;
            this.cosmosDbService = cosmosDbService;
        }

        // [HttpGet(Name = "GetBirdyVotes")]
        // public async Task<IEnumerable<BirdyWatch>> Get(string id)
        // {
        //     return await cosmosDbService.GetBirdyVotesAsync(id);
        // }

        // [HttpPost(Name = "VoteBirdy")]
        // public async Task<StatusCodeResult> Post(string id, bool correct)
        // {

        //     await cosmosDbService.VoteBirdy(id, correct);

        //     return Ok();
        // }
    }
}