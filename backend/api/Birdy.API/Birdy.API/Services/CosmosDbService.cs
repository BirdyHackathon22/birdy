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

        public async Task SetupBirdyWatch()
        {
            var toWrite = Enumerable.Range(1, 10).Select(index => this._container.CreateItemAsync(CreateRandomBirdyWatch()));
            await Task.WhenAll(toWrite);
        }

        private static BirdyWatch CreateRandomBirdyWatch()
        {
            var species = BirdSpecies.CommonBirds[Random.Shared.Next(BirdSpecies.CommonBirds.Length)];
            return new BirdyWatch(species, new Location(Random.Shared.NextInt64(29, 49), Random.Shared.NextInt64(-124, -73)))
            {
                DateSpotted = GetRandomDate(),
                ImageName = species,
                Score = Random.Shared.Next(0, 100),
                BoundingBox = "BoundingBox",
                Device = "FakeDevice"
            };
        }

        private static DateTime GetRandomDate()
        {
            return DateTime.Now.AddDays(Random.Shared.Next(-20, 0)).AddHours(Random.Shared.Next(0, 10)).AddMinutes(Random.Shared.Next(0, 10)).AddSeconds(Random.Shared.Next(0, 10));
        }

        #endregion

        public async Task<IEnumerable<BirdyWatch>> GetBirdyWatchesAsync()
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

        public async Task CreateBirdyWatch(BirdyWatch birdyWatch)
        {
            await this._container.CreateItemAsync(birdyWatch);
        }
    }
}
