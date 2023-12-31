using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Dto;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class RestaurantService: IRestaurantService
{
    private readonly ApplicationDBContext _context;
    private readonly IMenuService _menuService;

    public RestaurantService(ApplicationDBContext context, IMenuService menuService)
    {
        _menuService = menuService;
        _context = context;
    }

    public async Task<RestaurantPagedListDto> GetRestaurants(string? searchNameRestaurant, int page)
    {
        if (page < 1)
        {
            throw new NotCorrectDataException(message: "Page value must be greater than 0");
        }
        
        var restaurants = await _context.Restaurants
            .Select(restaurant => new RestaurantDTO
            {
                Id = restaurant.Id,
                Name = restaurant.Name
            })
            .ToListAsync();

        if (searchNameRestaurant != string.Empty)
        {
            restaurants = restaurants.Where(r => r.Name.ToLower().Contains(searchNameRestaurant?.Trim().ToLower() ?? string.Empty)).ToList();
        }

        foreach (var restaurant in restaurants)
        {
            restaurant.Menus = await GetMenus(restaurant.Id);
        }

        const int pageSize = 5;
        var restaurantsCount = restaurants.Count;
        var count = restaurantsCount % pageSize < pageSize && restaurantsCount % pageSize != 0
            ? restaurantsCount / 5 + 1
            : restaurantsCount / 5;

        if (page > count && restaurants.Any())
        {
            throw new NotCorrectDataException(message: "Invalid value for attribute page");
        }

        var items = restaurants.Skip((page - 1) * pageSize).Take((pageSize)).ToList();

        return new RestaurantPagedListDto
        {
            Restaurants = items,
            PageInfoModel = new PageInfoModelDTO(pageSize, restaurants.Count, page)
        };
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
            .FirstOrDefaultAsync();

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найдено ресторана с id = {restaurantId}");
        }
        
        restaurant.Menus = await GetMenus(restaurant.Id);

        return restaurant;
    }

    public async Task CreateRestaurant(CreateRestaurantDto model)
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

    public async Task UpdateRestaurant(Guid id, UpdateInfoRestaurantDto model)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);

        if (restaurant == null)
        {
            throw new NotFoundException($"Ресторан для внесения изменений с id = {id} не найден");
        }

        restaurant.Name = model.Name;
        
        _context.Restaurants.Attach(restaurant);
        _context.Entry(restaurant).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIsIdRestaurant(Guid id)
    {
        var restaurant = await _context.Restaurants
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        return restaurant != null;
    }

    public async Task AddCookToRestaurant(Guid cookId, Guid restaurantId)
    {
        var cook = await _context.Cooks.FindAsync(cookId);

        if (cook == null)
        {
            throw new NotFoundException($"Не найден повар с id = {cookId}");
        }
        
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найден ресторан с id = {restaurantId}");
        }

        restaurant.Cook = cook;
        
        _context.Restaurants.Attach(restaurant);
        _context.Entry(restaurant).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task AddManagerToRestaurant(Guid managerId, Guid restaurantId)
    {
        var manager = await _context.Managers.FindAsync(managerId);

        if (manager == null)
        {
            throw new NotFoundException($"Не найден повар с id = {managerId}");
        }
        
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найден ресторан с id = {restaurantId}");
        }

        restaurant.Manager = manager;
        
        _context.Restaurants.Attach(restaurant);
        _context.Entry(restaurant).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteManagerInRestaurant(Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найден ресторан с id = {restaurantId}");
        }

        restaurant.ManagerId = null;
        
        _context.Restaurants.Attach(restaurant);
        _context.Entry(restaurant).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCookInRestaurant(Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найден ресторан с id = {restaurantId}");
        }
        
        restaurant.CookId = null;
        
        _context.Restaurants.Attach(restaurant);
        _context.Entry(restaurant).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    private async Task<List<MenuDTO>> GetMenus(Guid restaurantId)
    {
        var menusEntity = await _context.Menus
            .Where(menu => menu.RestaurantId == restaurantId)
            .ToListAsync();

        var menusDto = new List<MenuDTO>();
        foreach (var menuEntity in menusEntity)
        {
            var menuDto = await _menuService.GetMenuDto(restaurantId, menuEntity.Id);
            menusDto.Add(menuDto);
        }

        return menusDto;
    }
}