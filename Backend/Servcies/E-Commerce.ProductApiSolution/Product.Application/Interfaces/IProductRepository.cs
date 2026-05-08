using Product.Domain.Entities;

namespace Product.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductEntity>> GetAllProductsAsync();

    Task<int?> GetStockAsync(string productId);
}