using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace testAPIs.Models;

public partial class TestDbContext : DbContext
{
    

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.ToTable("UserTable");

            entity.HasIndex(e => e.Email, "EmailColumn_UserTable").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "PhoneNumber_UserTable").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
