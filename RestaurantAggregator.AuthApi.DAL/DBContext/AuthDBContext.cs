using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.Etities;

namespace RestaurantAggregator.AuthApi.DAL.DBContext;

public class AuthDBContext: IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Courier> Couriers { get; set; }

    public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasOne(u => u.Customer)
            .WithOne(u => u.User)
            .HasForeignKey<Customer>(c => c.Id)
            .IsRequired();
        
        builder.Entity<User>()
            .HasOne(u => u.Manager)
            .WithOne(u => u.User)
            .HasForeignKey<Manager>(m => m.Id)
            .IsRequired();
        
        builder.Entity<User>()
            .HasOne(u => u.Cook)
            .WithOne(u => u.User)
            .HasForeignKey<Cook>(c => c.Id)
            .IsRequired();
        
        builder.Entity<User>()
            .HasOne(u => u.Courier)
            .WithOne(u => u.User)
            .HasForeignKey<Courier>(c => c.Id)
            .IsRequired();
    } 
}