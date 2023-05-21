using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class CartService: ICartService
{
    private readonly ApplicationDBContext _context;
    private readonly IUserService _userService;

    public CartService(ApplicationDBContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }
    
    public async Task<List<DishInCartDto>> GetCartDishes(Guid userId)
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

    public async Task<DishInCartDto> GetDishInCart(Guid userId, Guid dishId)
    {
        var dishInCart = await _context.DishesInCart
            .Where(d => d.DishId == dishId && d.CustomerId == userId)
            .FirstOrDefaultAsync();

        if (dishInCart == null)
            throw new NotFoundException($"Блюда с id = {dishId} не найдено в коризне у пользователя с id = {userId}");

        return GetDishInCartDto(dishInCart);
    }

    public async Task<DishInCartDto> AddDishInCart(Guid userId, Guid dishId)
    {
        var dishInCart = await _context.DishesInCart.FindAsync(dishId);

        if (dishInCart != null && dishInCart.CustomerId == userId)
        {
            dishInCart.Count += 1;
            
            await _context.SaveChangesAsync();
            return GetDishInCartDto(dishInCart);
        }
        
        var dish = await _context.Dishes.FindAsync(dishId);
        
        if (dish == null)
        {
            throw new NotFoundException(message: $"Блюдо с id = {dishId} не найдено");
        }
        
        await _context.Customers.FindAsync(userId);

        await _context.DishesInCart.AddAsync(new DishInCart
        {
            Count = 1,
            Dish = dish,
            Customer = new Customer
            {
                Id = userId
            }
        });
            
        await _context.SaveChangesAsync();
        return GetDishInCartDto(dishInCart!);
    }

    public async Task DeleteDishOfCart(Guid userId, Guid dishId)
    {
        var dishInCart = await _context.DishesInCart.FindAsync(dishId);
        
        if (dishInCart == null)
        {
            throw new NotFoundException(message: $"Блюдо с id = {dishId} не найдено в у пользователя с id = {userId} корзине");
        }

        _context.Remove(dishInCart);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeQuantity(Guid userId, Guid dishId, bool increase)
    {
        var dishInCart = await _context.DishesInCart.FindAsync(dishId);
        
        if (dishInCart == null)
        {
            throw new NotFoundException(message: $"Блюдо с id = {dishId} не найдено в у пользователя с id = {userId} корзине");
        }

        if (increase)
            dishInCart.Count += 1;
        else 
            dishInCart.Count -= 1;

        _context.DishesInCart.Attach(dishInCart);
        _context.Entry(dishInCart).State = EntityState.Modified;

        if (dishInCart.Count <= 0)
            _context.Remove(dishInCart);

        await _context.SaveChangesAsync();
    }

    public async Task ClearCart(Guid userId)
    {
        var dishesInCart = await _context.DishesInCart
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

        foreach (var dishInCart in dishesInCart)
        {
            _context.Remove(dishInCart);
        }
        
        await _context.SaveChangesAsync();
    }

    private DishInCartDto GetDishInCartDto(DishInCart dishInCart)
    {
        return new DishInCartDto
        {
            Id = dishInCart.Id,
            Count = dishInCart.Count,
            Dish = new DishDTO
            {
                Id = dishInCart.Dish.Id,
                Name = dishInCart.Dish.Name,
                Price = dishInCart.Dish.Price,
                Description = dishInCart.Dish.Description,
                IsVegetarian = dishInCart.Dish.IsVegetarian,
                Photo = dishInCart.Dish.Photo,
                Rating = dishInCart.Dish.Rating,
                Category = dishInCart.Dish.Category
            }
        };
    }
}