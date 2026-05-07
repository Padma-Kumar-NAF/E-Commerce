using Order.Domain.Entities;

namespace Order.Application.Interfaces;

public interface IOrderRepository
{
    Task Create(OrderEntity order);

    Task<List<OrderEntity>> GetAll();
}