using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Product.Infrastructure.Data
{
    public class CosmosDbInitializer
    {
        private readonly IConfiguration _configuration;

        public CosmosDbInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            var connectionString = _configuration["CosmosDb:ConnectionString"];
            var databaseName = _configuration["CosmosDb:DatabaseName"];
            var containerName = _configuration["CosmosDb:ContainerName"];

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

            using var client = new CosmosClient(connectionString, cosmosClientOptions);

            // Create database if it doesn't exist
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(
                databaseName,
                throughput: 400);

            Console.WriteLine($"Database '{databaseName}' created or already exists.");

            // Create container if it doesn't exist
            var containerProperties = new ContainerProperties
            {
                Id = containerName,
                PartitionKeyPath = "/id"
            };

            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(
                containerProperties,
                throughput: 400);

            Console.WriteLine($"Container '{containerName}' created or already exists.");
        }
    }
}
