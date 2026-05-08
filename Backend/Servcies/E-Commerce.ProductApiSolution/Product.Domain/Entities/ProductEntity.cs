namespace Product.Domain.Entities;

public class ProductEntity
{
    public string id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
}