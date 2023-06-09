using Microsoft.EntityFrameworkCore;

namespace Album.Api.Database;

public class albumContext : DbContext
{
    private readonly IConfiguration _configuration;
    public albumContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }

    public DbSet<Album.Api.Models.Album> Albums { get; set; }
}
