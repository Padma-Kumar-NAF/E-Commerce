using Product.Application.Interfaces;
using Product.Domain.Entities;

namespace Product.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
    {
        return await _repository.GetAllProductsAsync();
    }

    public async Task<int?> GetStockAsync(string productId)
    {
        return await _repository.GetStockAsync(productId);
    }
}