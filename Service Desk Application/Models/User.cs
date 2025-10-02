using System;
using System.Collections.Generic;

namespace Service_Desk_Application.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Ticket> TicketAssignedToNavigations { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketCreateByNavigations { get; set; } = new List<Ticket>();
}
