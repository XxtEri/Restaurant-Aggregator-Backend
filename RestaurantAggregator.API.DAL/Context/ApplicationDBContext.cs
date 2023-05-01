using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.DAL.Entities;

namespace RestaurantAggregator.API.DAL;

public class ApplicationDBContext: DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}