using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;

namespace RestaurantAggregator.API.BL.Services;

public class RestaurantService: IRestaurantService
{
    private readonly ApplicationDBContext _context;

    public RestaurantService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<RestaurantDTO>> GetRestaurant()
    {
        var restaurants = await _context.Restaurants
            .Select(restaurant => new RestaurantDTO
            {
                Name = restaurant.Name
            })
            .ToListAsync();

        return restaurants;
    }

    // public List<MenuDTO> getMenus(Guid restaurantId)
    // {
    //     var menusEntity = _context.Restaurants
    //         .Where(restaurant => restaurant.Id == restaurantId)
    //         .Select(restaurant => restaurant.Menus)
    //         .ToList();
    //
    //     var menus = List<MenuDTO>();
    //
    //     foreach (var menu in menusEntity)
    //     {
    //         
    //     }
    //
    //     return menus;
    // } 
}