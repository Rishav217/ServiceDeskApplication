namespace Service_Desk_Application.DTOs;

public class UserDTO
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class DashboardStatsDTO
{
    public int TotalTickets { get; set; }
    public Dictionary<string, int> TicketsByStatus { get; set; } = new();
}
