using Microsoft.EntityFrameworkCore;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<StopPoint>? StopPoints { get; set; }
    public DbSet<Line>? Lines { get; set; }
}