using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Data;
using Service_Desk_Application.DTOs;
using Service_Desk_Application.Models;
using Service_Desk_Application.Services;

namespace Service_Desk_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ServiceDeskDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(ServiceDeskDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role.RoleName,
            UserId = user.UserId
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        // Check if username already exists
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return BadRequest(new { message = "Username already exists" });
        }

        // Get Guest role ID
        var guestRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Guest");
        if (guestRole == null)
        {
            return StatusCode(500, new { message = "Guest role not found in database" });
        }

        // Create new user with Guest role
        var newUser = new User
        {
            Username = request.Username,
            Password = request.Password, // In production, hash this password!
            RoleId = guestRole.RoleId,
            IsDeleted = false
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // Load the role for token generation
        await _context.Entry(newUser).Reference(u => u.Role).LoadAsync();

        var token = _jwtService.GenerateToken(newUser);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = newUser.Username,
            Role = newUser.Role.RoleName,
            UserId = newUser.UserId
        });
    }
}
