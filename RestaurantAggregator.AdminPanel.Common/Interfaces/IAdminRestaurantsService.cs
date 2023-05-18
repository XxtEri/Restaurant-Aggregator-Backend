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
}