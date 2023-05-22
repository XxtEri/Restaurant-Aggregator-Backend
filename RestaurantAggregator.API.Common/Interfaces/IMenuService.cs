using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IMenuService
{
    Task<MenuDTO> GetMenuDto(Guid restaurantId, Guid menuId);
    Task AddMenuToRestaurant(Guid restaurantId, CreateMenuDto model);
}