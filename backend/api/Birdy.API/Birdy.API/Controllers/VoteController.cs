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

        [HttpPut("{id}/true", Name = "VoteBirdyTrue")]
        public async Task<StatusCodeResult> PostTrue(string id)
        {

            await cosmosDbService.PutVote(id, true);

            return Ok();
        }

        [HttpPut("{id}/false", Name = "VoteBirdyFalse")]
        public async Task<StatusCodeResult> PostFalse(string id)
        {

            await cosmosDbService.PutVote(id, false);

            return Ok();
        }
    }
}