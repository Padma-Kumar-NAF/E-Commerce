using Product.Domain.Entities;

namespace Product.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductEntity>> GetAllProductsAsync();

    Task<int?> GetStockAsync(string productId);
}