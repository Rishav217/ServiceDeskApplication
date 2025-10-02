using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Data;
using Service_Desk_Application.Models;

namespace Service_Desk_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StatusesController : ControllerBase
{
    private readonly ServiceDeskDbContext _context;

    public StatusesController(ServiceDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/statuses
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<TicketStatus>>> GetStatuses()
    {
        return await _context.TicketStatuses.ToListAsync();
    }

    // GET: api/statuses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TicketStatus>> GetStatus(int id)
    {
        var status = await _context.TicketStatuses.FindAsync(id);

        if (status == null)
        {
            return NotFound();
        }

        return status;
    }

    // POST: api/statuses
    [HttpPost]
    public async Task<ActionResult<TicketStatus>> CreateStatus([FromBody] TicketStatus status)
    {
        status.IsDeleted = false;
        _context.TicketStatuses.Add(status);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStatus), new { id = status.StatusId }, status);
    }

    // PUT: api/statuses/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatus status)
    {
        if (id != status.StatusId)
        {
            return BadRequest();
        }

        _context.Entry(status).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.TicketStatuses.AnyAsync(e => e.StatusId == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/statuses/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStatus(int id)
    {
        var status = await _context.TicketStatuses.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.StatusId == id);

        if (status == null)
        {
            return NotFound();
        }

        status.IsDeleted = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
