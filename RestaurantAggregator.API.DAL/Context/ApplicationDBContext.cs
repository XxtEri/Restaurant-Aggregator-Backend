using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.DAL.Entities;

namespace RestaurantAggregator.API.DAL;

public class ApplicationDBContext: DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<MenuDish> MenusDishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<DishInCart> DishesInCart { get; set; }
    public DbSet<OrderDish> OrdersDishes { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Menus)
            .WithOne(r => r.Restaurant)
            .HasForeignKey(m => m.RestaurantId)
            .IsRequired();
        
        modelBuilder.Entity<Cook>()
            .HasOne(o => o.Restaurant)
            .WithOne(c => c.Cook)
            .HasForeignKey<Restaurant>(c => c.CookId);
        
        modelBuilder.Entity<Manager>()
            .HasOne(o => o.Restaurant)
            .WithOne(c => c.Manager)
            .HasForeignKey<Restaurant>(c => c.ManagerId);
        
        modelBuilder.Entity<MenuDish>()
            .HasKey(e => new {e.DishId, e.MenuId});
        
        modelBuilder.Entity<MenuDish>()
            .HasOne(e => e.Dish)
            .WithMany(d => d.MenusDishes)
            .HasForeignKey(e => e.DishId)
            .IsRequired();
        
        modelBuilder.Entity<MenuDish>()
            .HasOne(e => e.Menu)
            .WithMany(e => e.MenusDishes)
            .HasForeignKey(e => e.MenuId)
            .IsRequired();

        modelBuilder.Entity<DishInCart>()
            .HasOne(d => d.Dish)
            .WithMany(e => e.DishInCarts)
            .HasForeignKey(e => e.DishId)
            .IsRequired();
        
        modelBuilder.Entity<DishInCart>()
            .HasOne(d => d.Customer)
            .WithMany(e => e.DishInCarts)
            .HasForeignKey(e => e.CustomerId)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Cook)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CookId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Courier)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CourierId);
        
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);
        
        modelBuilder.Entity<OrderDish>()
            .HasOne(o => o.Order)
            .WithMany(c => c.OrderDishes)
            .HasForeignKey(o => o.OrderId);
        
        modelBuilder.Entity<OrderDish>()
            .HasOne(o => o.Dish)
            .WithMany(c => c.OrderDishes)
            .HasForeignKey(o => o.DishId);
    }
}