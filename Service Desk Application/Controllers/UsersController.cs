using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Data;
using Service_Desk_Application.DTOs;

namespace Service_Desk_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly ServiceDeskDbContext _context;

    public UsersController(ServiceDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .OrderBy(u => u.UserId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                RoleId = u.RoleId,
                RoleName = u.Role.RoleName
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/users/support-engineers
    [HttpGet("support-engineers")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetSupportEngineers()
    {
        var supportEngineers = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.RoleName == "SupportEngineer")
            .Select(u => new UserDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                RoleId = u.RoleId,
                RoleName = u.Role.RoleName
            })
            .ToListAsync();

        return Ok(supportEngineers);
    }

    // PUT: api/users/5/change-role
    [HttpPut("{id}/change-role")]
    public async Task<IActionResult> ChangeUserRole(int id, [FromBody] int newRoleId)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Don't allow changing Admin role
        if (user.Role.RoleName == "Admin")
        {
            return BadRequest(new { message = "Cannot change admin user role" });
        }

        user.RoleId = newRoleId;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
