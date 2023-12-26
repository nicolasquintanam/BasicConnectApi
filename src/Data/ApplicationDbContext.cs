namespace BasicConnectApi.Data;

using Microsoft.EntityFrameworkCore;
using BasicConnectApi.Models;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<RevokedToken> RevokedToken { get; set; }

    public int SaveChanges() => base.SaveChanges();
    public async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUser(modelBuilder);
        ConfigureRevokedToken(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
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
            .Property(e => e.IsEmailConfirmed)
            .HasDefaultValue(false)
            .HasColumnName("is_email_confirmed");

        modelBuilder.Entity<User>()
            .Property(e => e.EmailConfirmationToken)
            .HasMaxLength(50)
            .IsRequired(false)
            .HasColumnName("email_confirmation_token");

        modelBuilder.Entity<User>()
            .Property(e => e.Password)
            .HasMaxLength(64)
            .HasColumnName("password");

        modelBuilder.Entity<User>()
            .HasMany(u => u.RevokedTokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .ToTable("user");
    }

    private static void ConfigureRevokedToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RevokedToken>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<RevokedToken>()
            .Property(e => e.Id)
            .HasColumnName("id");

        modelBuilder.Entity<RevokedToken>()
            .Property(t => t.TokenId)
            .IsRequired()
            .HasColumnName("token_id");

        modelBuilder.Entity<RevokedToken>()
            .Property(t => t.UserId)
            .HasColumnName("user_id");

        modelBuilder.Entity<RevokedToken>()
            .ToTable("revoked_token");
    }
}
