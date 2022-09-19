using Birdy.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Birdy.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {
        private static readonly string[] CommonBirds = new[]
        {
            "Mourning Dove", "Northern Cardinal", "American Robin", "American Crow", "Blue Jay", "Song Sparrow", "Red-winged Blackbird", "European Starling", "American Goldfinch", "Canada Goose"
        };

        private readonly ILogger<AnimalController> _logger;

        public AnimalController(ILogger<AnimalController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Animal> Get()
        {
            return Enumerable.Range(1, 7).Select(index => new Animal(CommonBirds[Random.Shared.Next(CommonBirds.Length)],
                new Location(Random.Shared.NextInt64(29, 49), Random.Shared.NextInt64(-124, -73)))
            {
                LastBirdyWatch = DateTime.Now.AddDays(Random.Shared.Next(-20,0)).AddHours(Random.Shared.Next(0, 10)).AddMinutes(Random.Shared.Next(0, 10)).AddSeconds(Random.Shared.Next(0, 10)),

            })
            .ToArray();
        }
    }
}