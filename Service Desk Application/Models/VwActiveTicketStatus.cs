using System;
using System.Collections.Generic;

namespace Service_Desk_Application.Models;

public partial class VwActiveTicketStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }
}
