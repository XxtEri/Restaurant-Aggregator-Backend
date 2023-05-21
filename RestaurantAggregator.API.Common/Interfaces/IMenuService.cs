using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IMenuService
{
    Task<MenuDTO> AddMenuToRestaurant(Guid restaurantId, CreateMenuDto model);
}