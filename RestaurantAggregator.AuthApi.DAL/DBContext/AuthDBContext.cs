using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.Etities;

namespace RestaurantAggregator.AuthApi.DAL.DBContext;

public class AuthDBContext: IdentityDbContext<User, IdentityRole<long>, long>
{
    private readonly IUserService _userService;
    
    public DbSet<User> Users { get; set; }

    public AuthDBContext(DbContextOptions<AuthDBContext> options): base(options)
    {
        Database.EnsureCreated();
    }
}