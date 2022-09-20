using Birdy.API.Models;
using Birdy.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Birdy.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly ILogger<MediaController> _logger;

        public MediaController(ILogger<MediaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{filename}", Name = "GetMedia")]
        public async Task<FileContentResult> Get(string filename)
        {
            byte[] fileContents = null;

            using (var client = new HttpClient())
            {
                fileContents = await client.GetByteArrayAsync(BirdSpecies.Images[filename]);
            }

            string contentType = "image/jpeg";

            return File(fileContents, contentType);
        }
    }
}
