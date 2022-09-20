using Birdy.API.Models;
using Birdy.API.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Birdy.API.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        #region Setup

        private static readonly string[] CommonBirds = new[]
        {
            "Mourning Dove", "Northern Cardinal", "American Robin", "American Crow", "Blue Jay", "Song Sparrow", "Red-winged Blackbird", "European Starling", "American Goldfinch", "Canada Goose"
        };

        public async Task SetupAnimal()
        {
            var toWrite = Enumerable.Range(1, 10).Select(index => 
                this._container.CreateItemAsync(
                    new BirdyWatch(CommonBirds[Random.Shared.Next(CommonBirds.Length)],
                        new Location(Random.Shared.NextInt64(29, 49), Random.Shared.NextInt64(-124, -73))) { LastWatchDate = GetRandomDate(), Id = Guid.NewGuid().ToString() }
                    )
            );
            await Task.WhenAll(toWrite);
        }

        private static DateTime GetRandomDate()
        {
            return DateTime.Now.AddDays(Random.Shared.Next(-20, 0)).AddHours(Random.Shared.Next(0, 10)).AddMinutes(Random.Shared.Next(0, 10)).AddSeconds(Random.Shared.Next(0, 10));
        }

        #endregion

        public async Task<IEnumerable<BirdyWatch>> GetAnimalsAsync()
        {
            var result = new List<BirdyWatch>();

            var iterator = this._container.GetItemLinqQueryable<BirdyWatch>().ToFeedIterator();

            while (iterator.HasMoreResults)
            {
                var feedResponse = await iterator.ReadNextAsync();
                foreach (var animal in feedResponse)
                {
                    if (animal is null)
                        continue;

                    result.Add(animal);
                }
            }

            return result;
        }
    }
}
