using System;
using System.Collections.Generic;

namespace Service_Desk_Application.Models;

public partial class VwActivePriority
{
    public int PriorityId { get; set; }

    public string PriorityName { get; set; } = null!;

    public string? Description { get; set; }
}
