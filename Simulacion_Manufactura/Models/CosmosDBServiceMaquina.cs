using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulacion_Manufactura.Models
{
    public interface ICosmosDBServiceMaquina
    {
        Task<IEnumerable<Maquina>> GetMaquinasAsync(string query);
        Task<Maquina> GetMaquinaAsync(string id);
        Task AddMaquinaAsync(Maquina item);
        Task UpdateMaquinaAsync(string id, Maquina item);
        Task DeleteMaquinaAsync(string id);
    }
    public class CosmosDBServiceMaquina : ICosmosDBServiceMaquina
    {
        private Container _container;

        public CosmosDBServiceMaquina(CosmosClient client, string databaseName, string containerName)
        {
            this._container = client.GetContainer(databaseName, containerName);
        }

        public async Task AddMaquinaAsync(Maquina item)
        {
            await this._container.CreateItemAsync<Maquina>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteMaquinaAsync(string id)
        {
            await this._container.DeleteItemAsync<Maquina>(id, new PartitionKey(id));
        }

        public async Task<Maquina> GetMaquinaAsync(string id)
        {
            try
            {
                ItemResponse<Maquina> response = await this._container.ReadItemAsync<Maquina>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Maquina>> GetMaquinasAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Maquina>(new QueryDefinition(queryString));
            List<Maquina> results = new List<Maquina>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateMaquinaAsync(string id, Maquina item)
        {
            await this._container.UpsertItemAsync<Maquina>(item, new PartitionKey(id));
        }
    }
}
