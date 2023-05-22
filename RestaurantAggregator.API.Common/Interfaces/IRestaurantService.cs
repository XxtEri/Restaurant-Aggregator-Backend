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
    Task<bool> CheckIsIdRestaurant(Guid id);

    Task AddCookToRestaurant(Guid cookId, Guid restaurantId);
    Task AddManagerToRestaurant(Guid managerId, Guid restaurantId);
    
    Task DeleteManagerInRestaurant(Guid restaurantId);
    Task DeleteCookInRestaurant(Guid restaurantId);
}