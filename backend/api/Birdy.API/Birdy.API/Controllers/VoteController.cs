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

        // [HttpPut("{id}/correct", Name = "VoteBirdyCorrect")]
        // public async Task<StatusCodeResult> PostCorrect(string id)
        // {

        //     await cosmosDbService.VoteBirdy(id, true);

        //     return Ok();
        // }

        // [HttpPut("{id}/incorrect", Name = "VoteBirdyCorrect")]
        // public async Task<StatusCodeResult> PostIncorrect(string id)
        // {

        //     await cosmosDbService.VoteBirdy(id, false);

        //     return Ok();
        // }
    }
}