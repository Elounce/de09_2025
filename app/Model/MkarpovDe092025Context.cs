using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace app.Model;

public partial class MkarpovDe092025Context : DbContext
{
    public MkarpovDe092025Context()
    {
    }

    public MkarpovDe092025Context(DbContextOptions<MkarpovDe092025Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Cleaning> Cleanings { get; set; }

    public virtual DbSet<Floor> Floors { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=kolei.ru;database=mkarpov_de09_2025;uid=mkarpov;pwd=191004");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Cleaning>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PRIMARY");

            entity.ToTable("cleaning");

            entity.Property(e => e.RoomId)
                .ValueGeneratedOnAdd()
                .HasColumnName("roomId");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.Task)
                .HasColumnType("text")
                .HasColumnName("task");

            entity.HasOne(d => d.Room).WithOne(p => p.Cleaning)
                .HasForeignKey<Cleaning>(d => d.RoomId)
                .HasConstraintName("cleaning_ibfk_1");
        });

        modelBuilder.Entity<Floor>(entity =>
        {
            entity.HasKey(e => e.FloorId).HasName("PRIMARY");

            entity.ToTable("floor");

            entity.Property(e => e.FloorId).HasColumnName("floorId");
            entity.Property(e => e.FloorNumber)
                .HasMaxLength(10)
                .HasColumnName("floorNumber");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PRIMARY");

            entity.ToTable("guest");

            entity.HasIndex(e => e.RoomId, "roomId");

            entity.Property(e => e.GuestId).HasColumnName("guestId");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("datetime")
                .HasColumnName("departureDate");
            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime")
                .HasColumnName("entryDate");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.RoomId).HasColumnName("roomId");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");

            entity.HasOne(d => d.Room).WithMany(p => p.Guests)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("guest_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PRIMARY");

            entity.ToTable("room");

            entity.HasIndex(e => e.CategoryId, "categoryId");

            entity.HasIndex(e => e.FloorId, "floorId");

            entity.HasIndex(e => e.StatusId, "statusId");

            entity.Property(e => e.RoomId).HasColumnName("roomId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.FloorId).HasColumnName("floorId");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Category).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("room_ibfk_2");

            entity.HasOne(d => d.Floor).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.FloorId)
                .HasConstraintName("room_ibfk_1");

            entity.HasOne(d => d.Status).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("room_ibfk_3");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("status");

            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleId, "roleId");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.IsBlocked).HasColumnName("isBlocked");
            entity.Property(e => e.LastLoginDate)
                .HasColumnType("datetime")
                .HasColumnName("lastLoginDate");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.LoginAttempts).HasColumnName("loginAttempts");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
