using System;
using System.Collections.Generic;

namespace Service_Desk_Application.Models;

public partial class VwActiveUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;
}
