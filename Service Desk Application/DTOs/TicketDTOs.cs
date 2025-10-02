namespace Service_Desk_Application.DTOs;

public class TicketDTO
{
    public int TicketId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? InternalComments { get; set; }
    public int? AssignedTo { get; set; }
    public string? AssignedToUsername { get; set; }
    public int Priority { get; set; }
    public string PriorityName { get; set; } = string.Empty;
    public int Area { get; set; }
    public string AreaName { get; set; } = string.Empty;
    public int TicketStatus { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public int CreateBy { get; set; }
    public string CreateByUsername { get; set; } = string.Empty;
}

public class CreateTicketRequest
{
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; }
    public int Area { get; set; }
}

public class UpdateTicketRequest
{
    public string? Subject { get; set; }
    public string? Description { get; set; }
    public string? InternalComments { get; set; }
    public int? AssignedTo { get; set; }
    public int? Priority { get; set; }
    public int? Area { get; set; }
    public int? TicketStatus { get; set; }
    public DateTime? ClosedDate { get; set; }
}
