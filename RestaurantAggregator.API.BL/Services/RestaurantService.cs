using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.AuthApi.Common.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class RestaurantService: IRestaurantService
{
    private readonly ApplicationDBContext _context;

    public RestaurantService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<RestaurantDTO>> GetRestaurants()
    {
        var restaurants = await _context.Restaurants
            .Select(restaurant => new RestaurantDTO
            {
                Id = restaurant.Id,
                Name = restaurant.Name
            })
            .ToListAsync();
        
        foreach (var restaurant in restaurants)
        {
            restaurant.Menus = await GetMenus(restaurant.Id);
        }

        return restaurants;
    }
    
    public async Task<RestaurantDTO> GetRestaurant(Guid restaurantId)
    {
        var restaurant = await _context.Restaurants
            .Where(restaurant => restaurant.Id == restaurantId)
            .Select(restaurant => new RestaurantDTO
            {
                Id = restaurant.Id,
                Name = restaurant.Name
            })
            .FirstAsync();
        restaurant.Menus = await GetMenus(restaurant.Id);

        return restaurant;
    }

    public async Task CreateRestaurant(RestaurantDTO model)
    {
        await _context.Restaurants.AddAsync(new Restaurant
        {
            Name = model.Name
        });
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRestaurant(Guid restaurantId)
    {
        var restaurant = await _context.Restaurants
            .Where(restaurant => restaurant.Id == restaurantId)
            .FirstAsync();
        
        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRestaurant(RestaurantDTO model)
    {
        var restaurant = await _context.Restaurants.FindAsync(model.Id);

        if (restaurant == null)
        {
            throw new NotFoundElementException($"Ресторан для внесения изменений с id = {model.Id} не найден");
        }
        
        _context.Restaurants.Attach(restaurant);
        _context.Entry(restaurant).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    private async Task<List<MenuDTO>> GetMenus(Guid restaurantId)
    {
        return await _context.Menus
            .Where(menu => menu.RestaurantId == restaurantId)
            .Select(menu => new MenuDTO
            {
                Id = menu.Id,
                Name = menu.Name
            })
            .ToListAsync();
    }
}