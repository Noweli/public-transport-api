using Microsoft.EntityFrameworkCore;
using PublicTransportAPI.Data.Models;
using PublicTransportAPI.Data.Models.Auth;

namespace PublicTransportAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<StopPoint>? StopPoints { get; set; }
    public DbSet<Line>? Lines { get; set; }
    public DbSet<StopPointLineEvent>? StopPointLineEvents { get; set; }
    public DbSet<User> Users { get; set; }
}