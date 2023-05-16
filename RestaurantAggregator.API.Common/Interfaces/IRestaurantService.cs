using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IRestaurantService
{
    Task<RestaurantPagedListDto> GetRestaurants(string searchNameRestaurant, int page);
    Task<RestaurantDTO> GetRestaurant(Guid restaurantId);
    Task CreateRestaurant(CreateRestaurantDto model);
    Task DeleteRestaurant(Guid restaurantId);
    Task UpdateRestaurant(Guid id, UpdateInfoRestaurantDto model);
}