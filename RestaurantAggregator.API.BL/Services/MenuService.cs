using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class MenuService: IMenuService
{
    private readonly ApplicationDBContext _context;

    public MenuService(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<MenuDTO> AddMenuToRestaurant(Guid restaurantId, CreateMenuDto model)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найдено ресторана с id = {restaurantId}");
        }
        
        if (restaurant.Menus.ToList().Any(menu => menu.Name == model.Name))
        {
            throw new NotCorrectDataException($"У ресторана с id = {restaurantId} уже существует меню с таким названием");
        }

        var newMenu = new Menu
        {
            Name = model.Name,
            Restaurant = restaurant,
            MenusDishes = new List<MenuDish>()
        };
        
        restaurant.Menus.Add(newMenu);
        await _context.SaveChangesAsync();

        return new MenuDTO
        {
            Id = newMenu.Id,
            Name = newMenu.Name
        };
    }
}