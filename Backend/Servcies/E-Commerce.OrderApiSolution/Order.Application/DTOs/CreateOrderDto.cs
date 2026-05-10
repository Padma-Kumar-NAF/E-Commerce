namespace Order.Application.DTOs;

public class CreateOrderDto
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public decimal Price { get; set; }  
}