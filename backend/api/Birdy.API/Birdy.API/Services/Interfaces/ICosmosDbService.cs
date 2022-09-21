using Birdy.API.Models;
using Birdy.API.Models.Request;

namespace Birdy.API.Services.Interfaces
{
    public interface ICosmosDbService
    {
        Task CreateBirdyWatch(BirdyWatch birdyWatch);
        Task<IEnumerable<BirdyWatch>> GetBirdyWatchesAsync();
        Task<IEnumerable<BirdyWatch>> GetBirdyWatchesFilterAsync(QueryRequest query);
    }
}
