using Microsoft.AspNetCore.Mvc;
using Order.Application.DTOs;
using Order.Application.Interfaces;

namespace Order.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        await _service.CreateOrder(dto);

        return Ok("Order Created");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _service.GetOrders();

        return Ok(orders);
    }
}