using Birdy.API.Models;

namespace Birdy.API.Services.Interfaces
{
    public interface ICosmosDbService
    {
        Task CreateBirdyWatch(BirdyWatch birdyWatch);
        Task<IEnumerable<BirdyWatch>> GetBirdyWatchesAsync();
        Task<IEnumerable<BirdyWatch>> GetBirdyWatchesFilterAsync(string species, int score);
    }
}
