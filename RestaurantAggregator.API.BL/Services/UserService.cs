using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;

namespace RestaurantAggregator.API.BL.Services;

public class UserService: IUserService
{
    private readonly ApplicationDBContext _context;

    public UserService(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<Guid> AddNewCustomerToDb(Guid customerId)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == customerId)
            .FirstOrDefaultAsync();
        
        if (customer == null)
        {
            await _context.Customers.AddAsync(new Customer
            {
                Id = customerId
            });
            await _context.SaveChangesAsync();
        }

        return customer!.Id;
    }

    public string? GetUserIdFromToke(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        var userId = jwtToken?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        return userId;
    }
}