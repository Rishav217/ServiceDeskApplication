using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Data;
using Service_Desk_Application.DTOs;
using Service_Desk_Application.Models;
using System.Security.Claims;

namespace Service_Desk_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ServiceDeskDbContext _context;

    public TicketsController(ServiceDeskDbContext context)
    {
        _context = context;
    }

    // GET: api/tickets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        IQueryable<Ticket> query = _context.Tickets
            .Include(t => t.AreaNavigation)
            .Include(t => t.PriorityNavigation)
            .Include(t => t.TicketStatusNavigation)
            .Include(t => t.CreateByNavigation)
            .Include(t => t.AssignedToNavigation);

        // Filter based on role
        if (userRole == "Guest")
        {
            query = query.Where(t => t.CreateBy == currentUserId);
        }
        else if (userRole == "SupportEngineer")
        {
            query = query.Where(t => t.AssignedTo == currentUserId);
        }
        // Admin sees all tickets

        var tickets = await query
            .OrderByDescending(t => t.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TicketDTO
            {
                TicketId = t.TicketId,
                Subject = t.Subject,
                Description = t.Description,
                InternalComments = t.InternalComments,
                AssignedTo = t.AssignedTo,
                AssignedToUsername = t.AssignedToNavigation != null ? t.AssignedToNavigation.Username : null,
                Priority = t.Priority,
                PriorityName = t.PriorityNavigation.PriorityName,
                Area = t.Area,
                AreaName = t.AreaNavigation.AreaName,
                TicketStatus = t.TicketStatus,
                StatusName = t.TicketStatusNavigation.StatusName,
                CreateDate = t.CreateDate,
                ClosedDate = t.ClosedDate,
                CreateBy = t.CreateBy,
                CreateByUsername = t.CreateByNavigation.Username
            })
            .ToListAsync();

        return Ok(tickets);
    }

    // GET: api/tickets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDTO>> GetTicket(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.AreaNavigation)
            .Include(t => t.PriorityNavigation)
            .Include(t => t.TicketStatusNavigation)
            .Include(t => t.CreateByNavigation)
            .Include(t => t.AssignedToNavigation)
            .FirstOrDefaultAsync(t => t.TicketId == id);

        if (ticket == null)
        {
            return NotFound(new { message = "Ticket not found" });
        }

        var ticketDTO = new TicketDTO
        {
            TicketId = ticket.TicketId,
            Subject = ticket.Subject,
            Description = ticket.Description,
            InternalComments = ticket.InternalComments,
            AssignedTo = ticket.AssignedTo,
            AssignedToUsername = ticket.AssignedToNavigation?.Username,
            Priority = ticket.Priority,
            PriorityName = ticket.PriorityNavigation.PriorityName,
            Area = ticket.Area,
            AreaName = ticket.AreaNavigation.AreaName,
            TicketStatus = ticket.TicketStatus,
            StatusName = ticket.TicketStatusNavigation.StatusName,
            CreateDate = ticket.CreateDate,
            ClosedDate = ticket.ClosedDate,
            CreateBy = ticket.CreateBy,
            CreateByUsername = ticket.CreateByNavigation.Username
        };

        return Ok(ticketDTO);
    }

    // POST: api/tickets
    [HttpPost]
    public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] CreateTicketRequest request)
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        // Get "Not Assigned" status
        var notAssignedStatus = await _context.TicketStatuses
            .FirstOrDefaultAsync(s => s.StatusName == "Not Assigned");

        if (notAssignedStatus == null)
        {
            return StatusCode(500, new { message = "Default status 'Not Assigned' not found" });
        }

        var ticket = new Ticket
        {
            Subject = request.Subject,
            Description = request.Description,
            Priority = request.Priority,
            Area = request.Area,
            TicketStatus = notAssignedStatus.StatusId,
            CreateBy = currentUserId,
            CreateDate = DateTime.Now,
            IsDeleted = false
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        // Load navigation properties
        await _context.Entry(ticket).Reference(t => t.AreaNavigation).LoadAsync();
        await _context.Entry(ticket).Reference(t => t.PriorityNavigation).LoadAsync();
        await _context.Entry(ticket).Reference(t => t.TicketStatusNavigation).LoadAsync();
        await _context.Entry(ticket).Reference(t => t.CreateByNavigation).LoadAsync();

        var ticketDTO = new TicketDTO
        {
            TicketId = ticket.TicketId,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Priority = ticket.Priority,
            PriorityName = ticket.PriorityNavigation.PriorityName,
            Area = ticket.Area,
            AreaName = ticket.AreaNavigation.AreaName,
            TicketStatus = ticket.TicketStatus,
            StatusName = ticket.TicketStatusNavigation.StatusName,
            CreateDate = ticket.CreateDate,
            CreateBy = ticket.CreateBy,
            CreateByUsername = ticket.CreateByNavigation.Username
        };

        return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, ticketDTO);
    }

    // PUT: api/tickets/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketRequest request)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound(new { message = "Ticket not found" });
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Subject))
            ticket.Subject = request.Subject;

        if (!string.IsNullOrEmpty(request.Description))
            ticket.Description = request.Description;

        if (request.InternalComments != null)
            ticket.InternalComments = request.InternalComments;

        if (request.AssignedTo.HasValue)
            ticket.AssignedTo = request.AssignedTo.Value;

        if (request.Priority.HasValue)
            ticket.Priority = request.Priority.Value;

        if (request.Area.HasValue)
            ticket.Area = request.Area.Value;

        if (request.TicketStatus.HasValue)
            ticket.TicketStatus = request.TicketStatus.Value;

        if (request.ClosedDate.HasValue)
            ticket.ClosedDate = request.ClosedDate.Value;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/tickets/5 (Soft Delete)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.TicketId == id);

        if (ticket == null)
        {
            return NotFound(new { message = "Ticket not found" });
        }

        ticket.IsDeleted = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
