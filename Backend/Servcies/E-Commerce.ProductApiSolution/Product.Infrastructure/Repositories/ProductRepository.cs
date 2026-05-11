using Microsoft.Azure.Cosmos;
using Product.Application.Interfaces;
using Product.Domain.Entities;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly Container _container;

    public ProductRepository(CosmosDbContext context)
    {
        _container = context.ProductsContainer;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
    {
        var query = _container.GetItemQueryIterator<ProductEntity>("SELECT * FROM c");

        List<ProductEntity> products = new();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            products.AddRange(response);
        }

        return products;
    }

    public async Task<int?> GetStockAsync(string productId)
    {
        try
        {
            var response = await _container.ReadItemAsync<ProductEntity>(
                productId,
                new PartitionKey(productId));

            return response.Resource.Stock;
        }
        catch (CosmosException)
        {
            return null;
        }
    }
}