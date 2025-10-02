using System;
using System.Collections.Generic;

namespace Service_Desk_Application.Models;

public partial class VwActiveArea
{
    public int AreaId { get; set; }

    public string AreaName { get; set; } = null!;

    public string? Description { get; set; }
}
