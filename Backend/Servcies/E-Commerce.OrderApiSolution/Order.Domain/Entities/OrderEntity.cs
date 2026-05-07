using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Entities;

public class OrderEntity
{
    [Key]
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Price { get; set; }
}