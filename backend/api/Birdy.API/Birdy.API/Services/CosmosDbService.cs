using System.Text;
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

            string queryString = "SELECT * FROM c where 1=1 ";

            if (query.Species is not null)
            {
                // Is this dangerous as its direct injection?
                string speciesString = "('" + string.Join("','", query.Species) + "')";
                queryString += "and c.species in " + speciesString;
            }

            if (query.ScoreRange is not null && query.ScoreRange.Min > 0)
            {
                queryString += " and c.score > @scoreMin";
            }
            if (query.ScoreRange is not null && query.ScoreRange.Max > query.ScoreRange.Min)
            {
                queryString += " and c.score < @scoreMax";
            }

            QueryDefinition queryDefinition = new QueryDefinition(queryString);

            if (query.ScoreRange is not null && query.ScoreRange.Min is not -1)
            {
                queryDefinition.WithParameter("@scoreMin", query.ScoreRange.Min);

            }

            if (query.ScoreRange is not null && query.ScoreRange.Max is not -1)
            {
                queryDefinition.WithParameter("@scoreMax", query.ScoreRange.Max);
            }




            var iterator = this._container.GetItemQueryIterator<BirdyWatch>(queryDefinition);

            while (iterator.HasMoreResults)
            {
                var feedResponse = await iterator.ReadNextAsync();
                foreach (var animal in feedResponse)
                {
                    if (animal is null)
                        continue;

                    if (query.DateRange is not null && query.DateRange.Min != default(DateTime) && animal.DateSpotted < query.DateRange.Min)
                        continue;

                    if (query.DateRange is not null && query.DateRange.Max != default(DateTime) && animal.DateSpotted > query.DateRange.Max)
                        continue;


                    result.Add(animal);
                }
            }

            return result;
        }

        public async Task<ItemResponse<BirdyWatch>> PutVote(string id, bool isCorrect)
        {
            string vote = isCorrect ? "true_classification" : "false_classification";
            ItemResponse<BirdyWatch> response = await this._container.PatchItemAsync<BirdyWatch>(
                    id: id,
                    partitionKey: new PartitionKey(id),
                    patchOperations: new[] {
                            PatchOperation.Increment("/speciesvote/"+vote, 1) });

            return response;
        }
    }
}
