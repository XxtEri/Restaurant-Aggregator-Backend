using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IAdminRestaurantsService: IBaseCrudService<RestaurantDTO>
{
    public Task Create(CreateRestaurantDto model);
}