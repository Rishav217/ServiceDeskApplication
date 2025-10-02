using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Data;
using Service_Desk_Application.DTOs;

namespace Service_Desk_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly ServiceDeskDbContext _context;

    public DashboardController(ServiceDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/dashboard/stats
    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDTO>> GetDashboardStats()
    {
        var totalTickets = await _context.Tickets.CountAsync();

        var ticketsByStatus = await _context.Tickets
            .Include(t => t.TicketStatusNavigation)
            .GroupBy(t => t.TicketStatusNavigation.StatusName)
            .Select(g => new { StatusName = g.Key, Count = g.Count() })
            .ToListAsync();

        var stats = new DashboardStatsDTO
        {
            TotalTickets = totalTickets,
            TicketsByStatus = ticketsByStatus.ToDictionary(x => x.StatusName, x => x.Count)
        };

        return Ok(stats);
    }
}
