using Birdy.Web.Server.Services.Interfaces;
using Birdy.Web.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Birdy.Web.Server.Services
{
    public class BirdyApiService : IBirdyApiService
    {
        private readonly HttpClient client;

        public BirdyApiService(HttpClient client, ApiConfiguration apiConfiguration)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(apiConfiguration.BaseUrl);
        }

        public async Task<IEnumerable<BirdyWatch>> GetBirdyWatches()
        {
            var result = await client.GetFromJsonAsync<IEnumerable<BirdyWatch>>("BirdyWatch") ?? new List<BirdyWatch>();

            foreach (var item in result)
            {
                item.ImageUrl = GetImageUrl(item.ImageName);
            }

            return result;
        }

        public string GetImageUrl(string filename)
        {
            return $"{this.client.BaseAddress}Media/{filename}";
        }
    }
}
