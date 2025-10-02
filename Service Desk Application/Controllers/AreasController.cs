using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Data;
using Service_Desk_Application.Models;

namespace Service_Desk_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AreasController : ControllerBase
{
    private readonly ServiceDeskDbContext _context;

    public AreasController(ServiceDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/areas
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
    {
        return await _context.Areas.ToListAsync();
    }

    // GET: api/areas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Area>> GetArea(int id)
    {
        var area = await _context.Areas.FindAsync(id);

        if (area == null)
        {
            return NotFound();
        }

        return area;
    }

    // POST: api/areas
    [HttpPost]
    public async Task<ActionResult<Area>> CreateArea([FromBody] Area area)
    {
        area.IsDeleted = false;
        _context.Areas.Add(area);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArea), new { id = area.AreaId }, area);
    }

    // PUT: api/areas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArea(int id, [FromBody] Area area)
    {
        if (id != area.AreaId)
        {
            return BadRequest();
        }

        _context.Entry(area).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Areas.AnyAsync(e => e.AreaId == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/areas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArea(int id)
    {
        var area = await _context.Areas.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.AreaId == id);

        if (area == null)
        {
            return NotFound();
        }

        area.IsDeleted = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
