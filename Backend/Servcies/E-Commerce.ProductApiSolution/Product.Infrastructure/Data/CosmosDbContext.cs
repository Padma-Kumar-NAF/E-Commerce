using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Product.Infrastructure.Data
{
    public class CosmosDbContext
    {
        public Container ProductsContainer { get; }

        public CosmosDbContext(IConfiguration configuration)
        {
            var connectionString =
                configuration["CosmosDb:ConnectionString"];

            var databaseName =
                configuration["CosmosDb:DatabaseName"];

            var containerName =
                configuration["CosmosDb:ContainerName"];

            CosmosClient client = new CosmosClient(connectionString);

            ProductsContainer = client.GetContainer(databaseName, containerName);
        }
    }
}