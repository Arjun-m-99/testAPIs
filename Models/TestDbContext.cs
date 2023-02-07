using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace testAPIs.Models;

public partial class TestDbContext : DbContext
{
    //public TestDbContext()
    //{
    //}

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserActivityTable> UserActivityTables { get; set; }

    public virtual DbSet<UserAddressTable> UserAddressTables { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=LAPTOP-8FNAGHOU;Database=testDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserActivityTable>(entity =>
        {
            entity.HasKey(e => e.RowNumber);

            entity.ToTable("UserActivityTable");

            entity.HasIndex(e => e.RowNumber, "IX_UserActivityTable");

            entity.Property(e => e.GeneratedToken).IsUnicode(false);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.TimeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Time&Date");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserActivityTables)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserActivityTable_UserTable");
        });

        modelBuilder.Entity<UserAddressTable>(entity =>
        {
            entity.HasKey(e => e.RowNumber).HasName("PK_UT_Addresses");

            entity.ToTable("UserAddressTable");

            entity.Property(e => e.AddedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.AddressLine1).IsUnicode(false);
            entity.Property(e => e.AddressLine2).IsUnicode(false);
            entity.Property(e => e.AddressLine3).IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName).IsUnicode(false);

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.UserAddressTables)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAddressTable_UserTable");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserTable_1");

            entity.ToTable("UserTable");

            entity.HasIndex(e => e.Email, "EmailColumn_UserTable").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "PhoneNumber_UserTable").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AadharNumber).IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Passport)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("passport");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('USER')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
