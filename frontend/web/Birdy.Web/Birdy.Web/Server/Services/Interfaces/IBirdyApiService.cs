using Birdy.Web.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Birdy.Web.Server.Services.Interfaces
{
    public interface IBirdyApiService
    {
        Task<IEnumerable<BirdyWatch>> GetBirdyWatches();
    }
}
