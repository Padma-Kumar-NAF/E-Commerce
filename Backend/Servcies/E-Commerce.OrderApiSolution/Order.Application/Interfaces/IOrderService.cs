using Order.Application.DTOs;
using Order.Domain.Entities;

namespace Order.Application.Interfaces;

public interface IOrderService
{
    Task CreateOrder(CreateOrderDto dto);
    Task<List<OrderEntity?>> GetOrders();
}