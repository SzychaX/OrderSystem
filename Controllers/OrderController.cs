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
            return BadRequest(new { Message = "Order must include at least one product." });
        }

        // Ustaw datę zamówienia
        order.OrderDate = DateTime.UtcNow;

        // Przetwarzaj produkty w zamówieniu
        foreach (var orderProduct in order.OrderProducts)
        {
            orderProduct.Order = null; // Usuń pełne odwołanie do obiektu `Order`
            orderProduct.OrderId = order.Id; // Przypisz `OrderId`
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
