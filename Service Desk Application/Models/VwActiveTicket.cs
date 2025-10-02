using System;
using System.Collections.Generic;

namespace Service_Desk_Application.Models;

public partial class VwActiveTicket
{
    public int TicketId { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? InternalComments { get; set; }

    public int? AssignedTo { get; set; }

    public string? AssignedToUsername { get; set; }

    public int Priority { get; set; }

    public string PriorityName { get; set; } = null!;

    public int Area { get; set; }

    public string AreaName { get; set; } = null!;

    public int TicketStatus { get; set; }

    public string StatusName { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? ClosedDate { get; set; }

    public int CreateBy { get; set; }

    public string CreateByUsername { get; set; } = null!;
}
