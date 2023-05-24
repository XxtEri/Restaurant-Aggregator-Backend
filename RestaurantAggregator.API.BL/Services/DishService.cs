using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class DishService: IDishService
{
    private readonly ApplicationDBContext _context;
    private readonly IUserService _userService;

    public DishService(ApplicationDBContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }
    
    public async Task<DishPagedListDTO> GetListAllDishesInRestaurant(Guid restaurantId, 
        List<DishCategory> categories, 
        bool vegetarian, 
        SortingDish sorting, 
        int page) 
    {
        if (page < 1)
        {
            throw new NotCorrectDataException(message: "Page value must be greater than 0");
        }
        
        var menus = await _context.Menus
            .Where(m => m.RestaurantId == restaurantId)
            .ToListAsync();
        
        if (!menus.Any())
        {
            throw new NotFoundException($"Блюда не найдены или отсутствуют в ресторане с id = {restaurantId}");
        }

        var dishesInRestaurant = new List<DishDTO>();
        foreach (var menu in menus)
        {
            var dishes = GetListDishDto(categories, vegetarian, menu.Id);
            dishes = SortingDishes(dishes, sorting);
            
            dishesInRestaurant.AddRange(dishes);
        }
        
        const int pageSize = 5;
        var countDishes = dishesInRestaurant.Count;
        var count = countDishes % pageSize < pageSize && countDishes % pageSize != 0 
            ? countDishes / 5 + 1 
            : countDishes / 5;

        if (page > count && dishesInRestaurant.Any())
        {
            throw new NotCorrectDataException(message: "Invalid value for attribute page");
        }

        var itemsDishes = dishesInRestaurant.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new DishPagedListDTO
        {
            Dishes = itemsDishes,
            PageInfoModel = new PageInfoModelDTO(pageSize, count, page)
        };
    }

    public async Task<DishPagedListDTO> GetListDishesInMenu(Guid restaurantId, 
        Guid menuId,
        List<DishCategory> categories, 
        bool vegetarian,
        SortingDish sorting,
        int page)
    {
        if (page < 1)
        {
            throw new NotCorrectDataException(message: "Page value must be greater than 0");
        }
        
        var dishes = await _context.MenusDishes
            .Where(e => e.MenuId == menuId && e.Menu.RestaurantId == restaurantId)
            .Select(e => e.Dish)
            .Select(d => new DishDTO
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Description = d.Description,
                IsVegetarian = d.IsVegetarian,
                Photo = d.Photo,
                Rating = d.Rating,
                Category = d.Category
            })
            .ToListAsync();
        
        var pageSize = 5;
        var countDishes = dishes.Count();
        var count = countDishes % pageSize < pageSize && countDishes % pageSize != 0 ? countDishes / 5 + 1 : countDishes / 5;

        if (page > count && dishes.Any())
        {
            throw new NotCorrectDataException(message: "Invalid value for attribute page");
        }

        var itemsDishes = dishes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new DishPagedListDTO
        {
            Dishes = itemsDishes,
            PageInfoModel = new PageInfoModelDTO(pageSize, count, page)
        };
    }

    public async Task<DishDTO> GetDishInformation(Guid dishId)
    {
        var dish = await _context.Dishes
            .Where(d => d.Id == dishId)
            .Select(d => new DishDTO
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Description = d.Description,
                IsVegetarian = d.IsVegetarian,
                Photo = d.Photo,
                Rating = d.Rating,
                Category = d.Category
            })
            .FirstAsync();

        if (dish == null)
        {
            throw new NotFoundException("Данное блюдо не найдено");
        }
        
        return dish;
    }

    //TODO: проверить
    public async Task<bool> CheckCurrentUserSetRatingToDish(Guid userId, Guid dishId)
    {
        var customer = await GetCustomer(userId);

        if (customer == null)
        {
            throw new NotFoundException("Покупатель не найден");
        }

        var dish = await _context.Dishes
            .FindAsync(dishId);

        if (dish == null)
        {
            throw new NotFoundException($"Блюдо с id = {dishId} не найдено в базе данных");
        }
        
        var orderDishes = await _context.OrdersDishes
            .Where(o => o.DishId == dishId)
            .ToListAsync();

        foreach (var orderDish in orderDishes)
        {
            var order = await _context.Orders
                .FindAsync(orderDish.OrderId);

            if (order?.CustomerId == userId && order.Status == OrderStatus.Delivered)
                return true;
        }

        return false;
    }

    //TODO: проверить
    public async Task SetRatingToDish(Guid userId, Guid dishId, int ratingScore)
    {
        var customer = await GetCustomer(userId);

        if (customer == null)
        {
            throw new NotFoundException($"Покупатель c id = {userId} не найден");
        }

        var check = await CheckCurrentUserSetRatingToDish(userId, dishId);

        if (!check)
            throw new ForbiddenException($"Покупатель с id = { userId } не может поставить рейтинг данному блюду, так как ниразу заказывал его");

        var rating = await _context.Ratings
            .Where(r => r.DishId == dishId && r.CustomerId == userId)
            .FirstOrDefaultAsync();

        if (rating != null)
        {
            rating.Value = ratingScore;
            
            _context.Ratings.Attach(rating);
            _context.Entry(rating).State = EntityState.Modified;
        }
        else
        {
            rating = new Rating
            {
                Value = ratingScore,
                DishId = dishId,
                CustomerId = userId
            };

            await _context.Ratings.AddAsync(rating);
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddDishToMenuOfRestaurant(Guid restaurantId, Guid menuId, CreateDishDto model)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException($"Не найдено ресторана с id = {restaurantId}");
        }
        
        var menu = await _context.Menus
            .FirstOrDefaultAsync(m => m.Id == menuId && m.RestaurantId == restaurantId);

        if (menu == null)
        { 
            throw new NotFoundException($"Не найдено меню с id = {menuId} в ресторане с id = {restaurantId}");
        }

        var dish = new Dish
        {
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            IsVegetarian = model.IsVegetarian,
            Photo = model.Photo,
            Category = model.Category
        };

        await _context.Dishes.AddAsync(dish);
        await _context.MenusDishes.AddAsync(new MenuDish
        {
            Dish = dish,
            Menu = menu
        });

        await _context.SaveChangesAsync();
    }

    private async Task<Customer?> GetCustomer(Guid userId)
    {
        var customer = await _context.Customers
            .FindAsync(userId);

        if (customer != null) return customer;
        var customerId = await _userService.AddNewCustomerToDb(userId);
        customer = await _context.Customers
            .FindAsync(customerId);

        return customer;
    }
    
    private IQueryable<DishDTO> GetListDishDto(List<DishCategory> categories, bool vegetarian, Guid menuId)
    {
        var isEmptyCategories = categories.Count == 0;
        
        var dishes = _context.MenusDishes
            .Where(e => e.MenuId == menuId)
            .Select(e => e.Dish)
            .Where(d => d.IsVegetarian == vegetarian && (isEmptyCategories || categories.Contains(d.Category)))
            .Select(d => new DishDTO
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Description = d.Description,
                IsVegetarian = d.IsVegetarian,
                Photo = d.Photo,
                Rating = d.Rating,
                Category = d.Category
            });
        
        return dishes;
    }
    
    private IQueryable<DishDTO> SortingDishes(IQueryable<DishDTO> dishes, SortingDish sorting)
    {
        return sorting switch
        {
            SortingDish.NameDesk => dishes.OrderByDescending(s => s.Name),
            SortingDish.PriceAsk => dishes.OrderBy(s => s.Price),
            SortingDish.PriceDesk => dishes.OrderByDescending(s => s.Price),
            SortingDish.RatingAsk => dishes.OrderBy(s => s.Rating),
            SortingDish.RatingDesk => dishes.OrderByDescending(s => s.Rating),
            _ => dishes.OrderBy(s => s.Name)
        };
    }
}