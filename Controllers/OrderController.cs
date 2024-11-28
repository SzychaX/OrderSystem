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
    public async Task<IActionResult> GetOrders([FromQuery] int? userId)
    {
        var query = _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.User)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(o => o.UserId == userId);
        }

        var orders = await query.ToListAsync();

        return Ok(orders);
    }


    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (order == null || order.OrderProducts == null || !order.OrderProducts.Any())
        {
            return BadRequest(new { Message = "Order must include at least one product." });
        }
        
        var user = await _context.Users.FindAsync(order.UserId);
        if (user == null)
        {
            return BadRequest(new { Message = "Invalid UserId. User does not exist." });
        }
        
        order.OrderDate = DateTime.UtcNow;

        // Usuwa pełne odwołania do Order w OrderProducts
        foreach (var orderProduct in order.OrderProducts)
        {
            orderProduct.Order = null;
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
