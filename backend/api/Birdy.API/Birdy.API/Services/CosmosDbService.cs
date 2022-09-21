using Birdy.API.Models;
using Birdy.API.Models.Request;
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

        public async Task<IEnumerable<BirdyWatch>> GetBirdyWatchesFilterAsync(QueryRequest query)
        {
            var result = new List<BirdyWatch>();


            QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.species = @key1 and c.score = @key2")
                .WithParameter("@key1", query.Species[0])
                .WithParameter("@key2", query.ScoreRange.Min);

            var iterator = this._container.GetItemQueryIterator<BirdyWatch>(queryDefinition);

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

        //         public async Task<IEnumerable<BirdyWatch>> PutVote(string id, bool isCorrect)
        //         {
        //             await ItemResponse < Product > response = await container.PatchItemAsync<Product>(
        //                     id: id,
        //                     partitionKey: new PartitionKey("road-bikes"),
        //                     patchOperations: new[] {
        //                     PatchOperation.Replace("/price", 355.45)
        //     }
        // );
        //         }
    }
}
