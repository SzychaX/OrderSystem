using dotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product) // Ładowanie produktów
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (order == null || order.OrderProducts == null || !order.OrderProducts.Any())
        {
            return BadRequest(new { Error = "Order must have at least one product." });
        }

        // Ustawienie daty zamówienia
        order.OrderDate = DateTime.UtcNow;

        // Dodanie zamówienia i jego produktów do kontekstu
        foreach (var orderProduct in order.OrderProducts)
        {
            orderProduct.Order = null; // Backend nie wymaga wypełnienia pełnego obiektu
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(order);
    }
}
