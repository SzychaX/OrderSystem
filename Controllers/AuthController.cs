using Microsoft.AspNetCore.Mvc;
using dotNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using dotNET.Interfaces;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            await _userService.RegisterUserAsync(model.Login, model.Password, model.FirstName, model.LastName, model.Email);
            return Ok(new { Message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var user = await _userService.AuthenticateUserAsync(model.Login, model.Password);
            if (user == null)
            {
                return Unauthorized(new { Message = "Nieprawidłowy login lub hasło" });
            }
            
            return Ok(new { Message = "Login successful", UserId = user.Id });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }
}

public class RegisterModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class LoginModel
{
    public string Login { get; set; }
    public string Password { get; set; }
}
