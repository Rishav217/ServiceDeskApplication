using System;
using System.Collections.Generic;

namespace Service_Desk_Application.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? InternalComments { get; set; }

    public int? AssignedTo { get; set; }

    public int Priority { get; set; }

    public int Area { get; set; }

    public int TicketStatus { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ClosedDate { get; set; }

    public int CreateBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Area AreaNavigation { get; set; } = null!;

    public virtual User? AssignedToNavigation { get; set; }

    public virtual User CreateByNavigation { get; set; } = null!;

    public virtual Priority PriorityNavigation { get; set; } = null!;

    public virtual TicketStatus TicketStatusNavigation { get; set; } = null!;
}
