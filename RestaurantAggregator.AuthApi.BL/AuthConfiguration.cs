using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAggregator.CommonFiles;

namespace RestaurantAggregator.AuthApi.BL;

public static class AuthConfiguration
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            RoleManager<IdentityRole<Guid>> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            
            if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Customer));
            }
            
            if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Manager));
            }
            
            if (!await roleManager.RoleExistsAsync(UserRoles.Cook))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Cook));
            }
            
            if (!await roleManager.RoleExistsAsync(UserRoles.Courier))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Courier));
            }
        }
    }
}