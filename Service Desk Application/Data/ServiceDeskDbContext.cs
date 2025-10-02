using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Service_Desk_Application.Models;

namespace Service_Desk_Application.Data;

public partial class ServiceDeskDbContext : DbContext
{
    public ServiceDeskDbContext()
    {
    }

    public ServiceDeskDbContext(DbContextOptions<ServiceDeskDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketStatus> TicketStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwActiveArea> VwActiveAreas { get; set; }

    public virtual DbSet<VwActivePriority> VwActivePriorities { get; set; }

    public virtual DbSet<VwActiveTicket> VwActiveTickets { get; set; }

    public virtual DbSet<VwActiveTicketStatus> VwActiveTicketStatuses { get; set; }

    public virtual DbSet<VwActiveUser> VwActiveUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Areas__70B820486BE1146E");

            entity.HasIndex(e => e.IsDeleted, "IX_Areas_IsDeleted");

            entity.HasIndex(e => e.AreaName, "UQ__Areas__8EB6AF57BF63A130").IsUnique();

            entity.Property(e => e.AreaName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            // Global query filter for soft delete
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.HasKey(e => e.PriorityId).HasName("PK__Prioriti__D0A3D0BE7700C810");

            entity.HasIndex(e => e.IsDeleted, "IX_Priorities_IsDeleted");

            entity.HasIndex(e => e.PriorityName, "UQ__Prioriti__346EBED667264E05").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.PriorityName).HasMaxLength(50);
            
            // Global query filter for soft delete
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1ABCA5D839");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616063367A89").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC60725E02FA8");

            entity.ToTable(tb => tb.HasTrigger("trg_Tickets_EnsureSequentialId"));

            entity.HasIndex(e => e.Area, "IX_Tickets_Area");

            entity.HasIndex(e => e.AssignedTo, "IX_Tickets_AssignedTo");

            entity.HasIndex(e => e.CreateBy, "IX_Tickets_CreateBy");

            entity.HasIndex(e => e.CreateDate, "IX_Tickets_CreateDate");

            entity.HasIndex(e => e.IsDeleted, "IX_Tickets_IsDeleted");

            entity.HasIndex(e => e.Priority, "IX_Tickets_Priority");

            entity.HasIndex(e => e.TicketStatus, "IX_Tickets_TicketStatus");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Subject).HasMaxLength(200);

            entity.HasOne(d => d.AreaNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Area)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Areas");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TicketAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_Tickets_AssignedTo");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.TicketCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_CreateBy");

            entity.HasOne(d => d.PriorityNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Priority)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Priorities");

            entity.HasOne(d => d.TicketStatusNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)

        modelBuilder.Entity<TicketStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__TicketSt__C8EE206397AB07FF");

            entity.HasIndex(e => e.IsDeleted, "IX_TicketStatuses_IsDeleted");

            entity.HasIndex(e => e.StatusName, "UQ__TicketSt__05E7698A2D233CC3").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C8A49CD78");

            entity.HasIndex(e => e.IsDeleted, "IX_Users_IsDeleted");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.Username, "IX_Users_Username");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4C086ADA8").IsUnique();

            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<VwActiveArea>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActiveAreas");

            entity.Property(e => e.AreaId).ValueGeneratedOnAdd();
            entity.Property(e => e.AreaName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<VwActivePriority>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActivePriorities");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.PriorityId).ValueGeneratedOnAdd();
            entity.Property(e => e.PriorityName).HasMaxLength(50);
        });

        modelBuilder.Entity<VwActiveTicket>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActiveTickets");

            entity.Property(e => e.AreaName).HasMaxLength(100);
            entity.Property(e => e.AssignedToUsername).HasMaxLength(100);
            entity.Property(e => e.CreateByUsername).HasMaxLength(100);
            entity.Property(e => e.PriorityName).HasMaxLength(50);
            entity.Property(e => e.StatusName).HasMaxLength(50);
            entity.Property(e => e.Subject).HasMaxLength(200);
        });

        modelBuilder.Entity<VwActiveTicketStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActiveTicketStatuses");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.StatusId).ValueGeneratedOnAdd();
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<VwActiveUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActiveUsers");

            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
