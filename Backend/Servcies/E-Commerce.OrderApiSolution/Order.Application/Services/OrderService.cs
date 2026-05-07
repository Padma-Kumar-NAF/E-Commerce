using Order.Application.DTOs;
using Order.Application.Interfaces;
using Order.Domain.Entities;

namespace Order.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateOrder(CreateOrderDto dto)
    {
        var order = new OrderEntity
        {
            ProductId = dto.ProductId,
            UserId = dto.UserId,
            Price = dto.Price
        };

        await _repository.Create(order);
    }

    public async Task<List<OrderEntity>> GetOrders()
    {
        return await _repository.GetAll();
    }
}