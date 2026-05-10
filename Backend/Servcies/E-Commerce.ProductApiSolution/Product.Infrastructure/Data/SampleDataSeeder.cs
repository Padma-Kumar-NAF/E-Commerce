using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Product.Domain.Entities;
using System.Net.Http;

namespace Product.Infrastructure.Data
{
    public class SampleDataSeeder
    {
        private readonly Container _container;

        public SampleDataSeeder(Container container)
        {
            _container = container;
        }

        public async Task SeedAsync()
        {
            // Check if container already has data
            var query = _container.GetItemQueryIterator<int>("SELECT VALUE COUNT(1) FROM c");
            var count = 0;
            
            if (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                count = response.FirstOrDefault();
            }

            if (count > 0)
            {
                Console.WriteLine($"Container already has {count} products. Skipping seed.");
                return;
            }

            Console.WriteLine("Seeding sample products...");

            var sampleProducts = new[]
            {
                new ProductEntity
                {
                    id = "product-001",
                    Name = "Wireless Mouse",
                    Category = "Electronics",
                    Price = 29.99m,
                    Stock = 150,
                    ImageUrl = "https://example.com/images/wireless-mouse.jpg"
                },
                new ProductEntity
                {
                    id = "product-002",
                    Name = "Mechanical Keyboard",
                    Category = "Electronics",
                    Price = 89.99m,
                    Stock = 75,
                    ImageUrl = "https://example.com/images/mechanical-keyboard.jpg"
                },
                new ProductEntity
                {
                    id = "product-003",
                    Name = "USB-C Hub",
                    Category = "Accessories",
                    Price = 45.50m,
                    Stock = 200,
                    ImageUrl = "https://example.com/images/usb-c-hub.jpg"
                },
                new ProductEntity
                {
                    id = "product-004",
                    Name = "Laptop Stand",
                    Category = "Accessories",
                    Price = 39.99m,
                    Stock = 120,
                    ImageUrl = "https://example.com/images/laptop-stand.jpg"
                },
                new ProductEntity
                {
                    id = "product-005",
                    Name = "Webcam HD",
                    Category = "Electronics",
                    Price = 69.99m,
                    Stock = 90,
                    ImageUrl = "https://example.com/images/webcam-hd.jpg"
                }
            };

            foreach (var product in sampleProducts)
            {
                try
                {
                    await _container.CreateItemAsync(product, new PartitionKey(product.id));
                    Console.WriteLine($"  ✓ Added: {product.Name}");
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    Console.WriteLine($"  - Skipped (already exists): {product.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ Error adding {product.Name}: {ex.Message}");
                }
            }

            Console.WriteLine($"Sample data seeding completed. Added {sampleProducts.Length} products.");
        }

        public static async Task SeedFromConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration["CosmosDb:ConnectionString"];
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];

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
            var container = client.GetContainer(databaseName, containerName);

            var seeder = new SampleDataSeeder(container);
            await seeder.SeedAsync();
        }
    }
}
