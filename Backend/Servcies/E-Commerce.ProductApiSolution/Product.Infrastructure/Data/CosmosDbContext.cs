using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

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

            // Configure CosmosClient options for emulator
            var cosmosClientOptions = new CosmosClientOptions
            {
                HttpClientFactory = () =>
                {
                    HttpMessageHandler httpMessageHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (req, cert, chain, errors) => true
                    };

                    return new HttpClient(httpMessageHandler);
                },
                ConnectionMode = ConnectionMode.Gateway,
                LimitToEndpoint = true
            };

            CosmosClient client = new CosmosClient(connectionString, cosmosClientOptions);

            ProductsContainer = client.GetContainer(databaseName, containerName);
        }
    }
}