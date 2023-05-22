using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Exceptions;

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

    public async Task AddNewCookToDb(Guid cookId)
    {
        var cook = await _context.Cooks
            .Where(c => c.Id == cookId)
            .FirstOrDefaultAsync();
        
        if (cook == null)
        {
            await _context.Cooks.AddAsync(new Cook
            {
                Id = cookId
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddNewManagerToDb(Guid managerId)
    {
        var manager = await _context.Managers
            .Where(c => c.Id == managerId)
            .FirstOrDefaultAsync();
        
        if (manager == null)
        {
            await _context.Managers.AddAsync(new Manager
            {
                Id = managerId
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddNewCourierToDb(Guid courierId)
    {
        var courier = await _context.Couriers
            .Where(c => c.Id == courierId)
            .FirstOrDefaultAsync();
        
        if (courier == null)
        {
            await _context.Couriers.AddAsync(new Courier
            {
                Id = courierId
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCookFromDb(Guid cookId)
    {
        var cook = await _context.Cooks
            .Where(c => c.Id == cookId)
            .FirstOrDefaultAsync();
        
        if (cook != null)
        {
            _context.Cooks.Remove(cook);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteManagerFromDb(Guid managerId)
    {
        var manager = await _context.Managers
            .Where(m => m.Id == managerId)
            .FirstOrDefaultAsync();
        
        if (manager != null)
        {
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCourierFromDb(Guid courierId)
    {
        var courier = await _context.Couriers
            .Where(c => c.Id == courierId)
            .FirstOrDefaultAsync();
        
        if (courier != null)
        {
            _context.Couriers.Remove(courier);
            await _context.SaveChangesAsync();
        }
    }

    public string? GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        var userId = jwtToken?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        return userId;
    }

    public async Task AddRestaurantIdForCook(Guid cookId, Guid restaurantId)
    {
        var cook = await _context.Cooks
            .Where(c => c.Id == cookId)
            .FirstOrDefaultAsync();
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        
        if (cook == null)
        {
            throw new NotFoundException($"Не найден повар с id = {cookId}");
        }

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найден ресторан с id = {restaurantId}");
        }
        
        cook.Restaurant = restaurant;
            
        _context.Cooks.Attach(cook);
        _context.Entry(cook).State = EntityState.Modified;
            
        await _context.SaveChangesAsync();
    }

    public async Task AddRestaurantIdForManager(Guid managerId, Guid restaurantId)
    {
        var manager = await _context.Managers
            .Where(c => c.Id == managerId)
            .FirstOrDefaultAsync();
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (manager == null)
        {
            throw new NotFoundException($"Не найден менеджер с id = {managerId}");
        }

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найден ресторан с id = {restaurantId}");
        }
        
        manager.RestaurantId = restaurant.Id;
            
        _context.Managers.Attach(manager);
        _context.Entry(manager).State = EntityState.Modified;
            
        await _context.SaveChangesAsync();
    }
}