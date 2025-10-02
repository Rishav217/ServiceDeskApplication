using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Models;

namespace Service_Desk_Application.Data;

public partial class ServiceDeskDbContext
{
    // Add global query filters for soft delete
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // Apply soft delete filter to all entities with IsDeleted property
        modelBuilder.Entity<Area>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Priority>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Ticket>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TicketStatus>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
    }
}
