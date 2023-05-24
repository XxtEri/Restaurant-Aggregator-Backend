using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class MenuService: IMenuService
{
    private readonly ApplicationDBContext _context;

    public MenuService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<MenuDTO> GetMenuDto(Guid restaurantId, Guid menuId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найдено ресторана с id = {restaurantId}");
        }
        
        var menu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == menuId && m.RestaurantId == restaurantId);

        if (menu == null)
        {
            throw new NotFoundException($"Не найдено меню с id = {menuId} в ресторане с id = {restaurantId}");
        }

        var dishesId = await _context.MenusDishes
            .Where(o => o.MenuId == menuId)
            .Select(o => o.DishId)
            .ToListAsync();

        var dishes = new List<DishDTO>();
        foreach (var dishId in dishesId)
        {
            var dish = await _context.Dishes.FindAsync(dishId);

            if (dish != null)
            {
                dishes.Add(new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Description = dish.Description,
                    IsVegetarian = dish.IsVegetarian,
                    Photo = dish.Photo,
                    Rating = dish.Rating,
                    Category = dish.Category
                });
            }
        }
        
        return new MenuDTO
        {
            Id = menu.Id,
            Name = menu.Name,
            Dishes = dishes
        };
    }
    
    public async Task AddMenuToRestaurant(Guid restaurantId, CreateMenuDto model)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найдено ресторана с id = {restaurantId}");
        }

        var menus = await _context.Menus
            .Where(menu => menu.RestaurantId == restaurantId)
            .ToListAsync();

        if (menus.Any(menu => menu.Name == model.Name))
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
    }

    public async  Task DeleteMenuFromRestaurant(Guid restaurantId, Guid menuId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найдено ресторана с id = {restaurantId}");
        }

        var menu = await _context.Menus
            .Where(menu => menu.Id == menuId && menu.RestaurantId == restaurantId)
            .FirstOrDefaultAsync();

        if (menu == null)
        {
            throw new NotFoundException($"Меню с id = {menuId}  у ресторана с id = {restaurantId} не найдено");
        }

        var menusDishes = await _context.MenusDishes
            .Where(m => m.MenuId == menuId)
            .ToListAsync();

        foreach (var menuDish in menusDishes)
        {
            _context.MenusDishes.Remove(menuDish);
        }
        
        _context.Menus.Remove(menu);
        await _context.SaveChangesAsync();

    }
}