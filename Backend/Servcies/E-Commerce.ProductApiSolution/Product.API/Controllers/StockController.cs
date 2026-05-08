using Microsoft.AspNetCore.Mvc;
using Product.Application.Interfaces;

namespace Product.API.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    private readonly IProductService _service;

    public StockController(IProductService service)
    {
        _service = service;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> CheckStock(string productId)
    {
        var stock =
            await _service.GetStockAsync(productId);

        if (stock == null)
            return NotFound();

        return Ok(new
        {
            ProductId = productId,
            Stock = stock
        });
    }
}