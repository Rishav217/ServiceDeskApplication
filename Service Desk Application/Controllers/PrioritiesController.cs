using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Data;
using Service_Desk_Application.Models;

namespace Service_Desk_Application.Controllers; 

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PrioritiesController : ControllerBase
{
    private readonly ServiceDeskDbContext _context;

    public PrioritiesController(ServiceDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/priorities
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Priority>>> GetPriorities()
    {
        return await _context.Priorities.ToListAsync();
    }

    // GET: api/priorities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Priority>> GetPriority(int id)
    {
        var priority = await _context.Priorities.FindAsync(id);

        if (priority == null)
        {
            return NotFound();
        }

        return priority;
    }

    // POST: api/priorities
    [HttpPost]
    public async Task<ActionResult<Priority>> CreatePriority([FromBody] Priority priority)
    {
        priority.IsDeleted = false;
        _context.Priorities.Add(priority);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPriority), new { id = priority.PriorityId }, priority);
    }

    // PUT: api/priorities/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePriority(int id, [FromBody] Priority priority)
    {
        if (id != priority.PriorityId)
        {
            return BadRequest();
        }

        _context.Entry(priority).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Priorities.AnyAsync(e => e.PriorityId == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/priorities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePriority(int id)
    {
        var priority = await _context.Priorities.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.PriorityId == id);

        if (priority == null)
        {
            return NotFound();
        }

        priority.IsDeleted = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
