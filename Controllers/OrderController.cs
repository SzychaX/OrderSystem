using dotNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotNET.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromBody] Order order)
    {
        order.OrderDate = DateTime.UtcNow;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return Ok();
    }
}