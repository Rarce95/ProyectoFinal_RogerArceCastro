using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Models
{
    public interface ICosmosDBServiceSimulacion
    {
        Task<IEnumerable<Simaulacion>> GetSimulacionesAsync(string query);
        Task<Simaulacion> GetSimulacionAsync(string id);
        Task AddSimulacionAsync(Simaulacion item);
        Task UpdateSimulacionAsync(string id, Simaulacion item);
        Task DeleteSimulacionAsync(string id);
    }
    public class CosmosDBServiceSimulacion : ICosmosDBServiceSimulacion
    {

        private Container _container;

        public CosmosDBServiceSimulacion(CosmosClient client, string databaseName, string containerName)
        {
            this._container = client.GetContainer(databaseName, containerName);
        }

        public async Task AddSimulacionAsync(Simaulacion item)
        {
            await this._container.CreateItemAsync<Simaulacion>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteSimulacionAsync(string id)
        {
            await this._container.DeleteItemAsync<Simaulacion>(id, new PartitionKey(id));
        }

        public async Task<Simaulacion> GetSimulacionAsync(string id)
        {
            try
            {
                ItemResponse<Simaulacion> response = await this._container.ReadItemAsync<Simaulacion>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Simaulacion>> GetSimulacionesAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Simaulacion>(new QueryDefinition(queryString));
            List<Simaulacion> results = new List<Simaulacion>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateSimulacionAsync(string id, Simaulacion item)
        {
            await this._container.UpsertItemAsync<Simaulacion>(item, new PartitionKey(id));
        }
    }
}
