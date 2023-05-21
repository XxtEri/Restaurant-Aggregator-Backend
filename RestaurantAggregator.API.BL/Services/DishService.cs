using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class DishService: IDishService
{
    private readonly ApplicationDBContext _context;

    public DishService(ApplicationDBContext context)
    {
        _context = context;
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
            var dishes = await _context.MenusDishes
                .Where(e => e.MenuId == menu.Id)
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

    public async Task<bool> CheckCurrentUserSetRatingToDish(Guid userId, Guid dishId)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == userId)
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            //TODO: добавить его в бд
        }

        //TODO: проверить заказывал ли блюдо

        return true;
    }

    public async Task SetRatingToDish(Guid userId, Guid dishId, int ratingScore)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == userId)
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            //TODO: добавить его в бд
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
        }
        else
        {
            rating = new Rating
            {
                Value = ratingScore,
                DishId = dishId,
                CustomerId = userId
            };
        }
        
        _context.Ratings.Attach(rating);
        _context.Entry(rating).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}