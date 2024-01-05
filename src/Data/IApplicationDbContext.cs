using BasicConnectApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicConnectApi.Data;

public interface IApplicationDbContext
{
    DbSet<User> User { get; set; }
    DbSet<RevokedToken> RevokedToken { get; set; }
    DbSet<OneTimePassword> OneTimePassword { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync();
}