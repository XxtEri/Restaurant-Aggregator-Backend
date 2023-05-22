using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface ICartService
{
    Task<List<DishInCartDto>> GetCartDishes(Guid userId);
    Task<DishInCartDto> GetDishInCart(Guid userId, Guid dishId);
    Task AddDishInCart(Guid userId, Guid dishId);
    Task DeleteDishOfCart(Guid userId, Guid dishId);
    Task ChangeQuantity(Guid userId, Guid dishId, bool increase);

    Task ClearCart(Guid userId);
}