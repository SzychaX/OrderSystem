using dotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // Pobieranie wszystkich użytkowników
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    // Rejestracja nowego użytkownika
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] User user)
    {
        // Walidacja unikalności loginu i emaila
        if (await _context.Users.AnyAsync(u => u.Login == user.Login))
        {
            return BadRequest("Login already exists.");
        }

        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            return BadRequest("Email already exists.");
        }

        // Hashowanie hasła
        user.PasswordHash = HashPassword(user.PasswordHash);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully.");
    }

    // Logowanie użytkownika
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Login == request.Login);

        if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid login or password.");
        }

        return Ok("Login successful.");
    }

    // Funkcja hashowania hasła
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    // Funkcja weryfikacji hasła
    private static bool VerifyPasswordHash(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }
}

// Model dla żądania logowania
public class LoginRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}
