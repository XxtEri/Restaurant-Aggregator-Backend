using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.DAL.Entities;

namespace RestaurantAggregator.API.DAL;

public class ApplicationDBContext: DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<MenuDish> MenusDishes { get; set; }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Menus)
            .WithOne(r => r.Restaurant)
            .HasForeignKey(m => m.RestaurantId)
            .IsRequired();
        
        modelBuilder.Entity<MenuDish>()
            .HasKey(e => new {e.DishId, e.MenuId});
        
        modelBuilder.Entity<MenuDish>()
            .HasOne(e => e.Dish)
            .WithMany(d => d.MenusDishes)
            .HasForeignKey(e => e.DishId);
        
        modelBuilder.Entity<MenuDish>()
            .HasOne(e => e.Menu)
            .WithMany(e => e.MenusDishes)
            .HasForeignKey(e => e.MenuId);
    }
}