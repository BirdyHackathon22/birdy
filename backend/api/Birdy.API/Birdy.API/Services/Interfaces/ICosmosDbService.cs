using Birdy.API.Models;

namespace Birdy.API.Services.Interfaces
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<BirdyWatch>> GetAnimalsAsync();
    }
}
