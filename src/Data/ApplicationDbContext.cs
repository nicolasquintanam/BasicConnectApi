namespace BasicConnectApi.Data;

using Microsoft.EntityFrameworkCore;
using BasicConnectApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<RevokedToken> RevokedToken { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .HasColumnName("id");

        modelBuilder.Entity<User>()
            .Property(e => e.FirstName)
            .HasMaxLength(100)
            .HasColumnName("first_name");

        modelBuilder.Entity<User>()
            .Property(e => e.LastName)
            .HasMaxLength(100)
            .HasColumnName("last_name");

        modelBuilder.Entity<User>()
            .Property(e => e.Email)
            .HasMaxLength(255)
            .HasColumnName("email");

        modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(e => e.Password)
            .HasMaxLength(64)
            .HasColumnName("password");

        modelBuilder.Entity<User>()
            .ToTable("user");



        modelBuilder.Entity<RevokedToken>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<RevokedToken>()
            .Property(e => e.Id)
            .HasColumnName("id");

        modelBuilder.Entity<RevokedToken>()
            .Property(t => t.Token)
            .IsRequired()
            .HasColumnName("token");

        modelBuilder.Entity<User>()
            .HasMany(u => u.RevokedTokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RevokedToken>()
            .Property(t => t.UserId)
            .HasColumnName("user_id");

        modelBuilder.Entity<RevokedToken>()
            .ToTable("revoked_token");
    }
}
