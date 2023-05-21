using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface ICartService
{
    Task<List<DishInCartDto>> GetBasketDishes(Guid userId);
    Task AddDishInBasket(Guid userId, Guid dishId);
    Task DeleteDishOfBasket(Guid userId, Guid dishId);
    Task ChangeQuantity(Guid userId, Guid dishId, bool increase);
}