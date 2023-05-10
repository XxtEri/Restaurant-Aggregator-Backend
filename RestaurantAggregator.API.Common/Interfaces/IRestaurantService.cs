using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IRestaurantService
{
    Task<List<RestaurantDTO>> GetRestaurants();
    Task<RestaurantDTO> GetRestaurant(Guid restaurantId);
    Task CreateRestaurant(RestaurantDTO model);
    Task DeleteRestaurant(Guid restaurantId);
    Task UpdateRestaurant(RestaurantDTO restaurant);
}