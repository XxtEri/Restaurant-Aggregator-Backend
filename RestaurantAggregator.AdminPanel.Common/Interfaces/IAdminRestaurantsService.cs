using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IAdminRestaurantsService
{
    Task<RestaurantDTO> Get(Guid id);
    Task<RestaurantPagedListDto> Select(string? searchingName, int page);
    Task Update(Guid id, RestaurantDTO model);
    Task Delete(Guid id);
    public Task Create(CreateRestaurantDto model);
    Task<bool> CheckIsIdRestaurant(Guid id);
    Task AddCookToRestaurant(Guid cookId, Guid restaurantId);
    Task AddManagerToRestaurant(Guid managerId, Guid restaurantId);
    Task DeleteManagerInRestaurant(Guid restaurantId);
    Task DeleteCookInRestaurant(Guid restaurantId);
}