using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class CartService: ICartService
{
    private readonly ApplicationDBContext _context;

    public CartService(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<DishInCartDto>> GetBasketDishes(Guid userId)
    {
        return await _context.DishesInCart
            .Where(d => d.CustomerId == userId)
            .Select(d => new DishInCartDto
            {
                Id = d.Id,
                Count = d.Count,
                Dish = new DishDTO
                {
                    Id = d.DishId,
                    Name = d.Dish.Name,
                    Price = d.Dish.Price,
                    Description = d.Dish.Description,
                    IsVegetarian = d.Dish.IsVegetarian,
                    Photo = d.Dish.Photo,
                    Rating = d.Dish.Rating,
                    Category = d.Dish.Category
                }
            }).ToListAsync();
    }

    public async Task AddDishInBasket(Guid userId, Guid dishId)
    {
        var dishInCart = await _context.DishesInCart.FindAsync(dishId);

        if (dishInCart != null && dishInCart.CustomerId == userId)
        {
            dishInCart.Count += 1;
            
            await _context.SaveChangesAsync();
            return;
        }
        
        var dish = await _context.Dishes.FindAsync(dishId);
        
        if (dish == null)
        {
            throw new NotFoundException(message: $"Блюдо с id = {dishId} не найдено");
        }
        
        var customer = await _context.Customers.FindAsync(userId);

        if (customer == null)
        {
            //TODO: добавить пользователя в бд
            throw new Exception();
        }

        await _context.DishesInCart.AddAsync(new DishInCart
        {
            Count = 1,
            Dish = dish,
            Customer = customer
        });
            
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDishOfBasket(Guid userId, Guid dishId)
    {
        
    }

    public async Task ChangeQuantity(Guid userId, Guid dishId, bool increase)
    {
        
    }
}