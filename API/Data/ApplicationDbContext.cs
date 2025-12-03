using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
    
    public DbSet<TreasureRequest> TreasureRequests { get; set; }
    public DbSet<TreasureResult> TreasureResults { get; set; }
    
    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<TreasureRequest>()
            .HasKey(s => s.Id);

        model.Entity<TreasureResult>()
            .HasKey(r => r.Id);

        model.Entity<TreasureRequest>()
            .HasMany(s => s.Results)
            .WithOne(r => r.TreasureRequest)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}