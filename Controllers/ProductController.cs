using dotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotNET.Controllers;

public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult> GetByCategory(string category)
    {
        var products = await _context.Products
            .Where(p => p.Category == category)
            .ToListAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct([FromBody] Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }
}