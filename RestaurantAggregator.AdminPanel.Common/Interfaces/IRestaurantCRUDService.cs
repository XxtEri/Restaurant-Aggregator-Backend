using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IRestaurantCrudService: IBaseCrudService<RestaurantDTO>
{
    public Task Create(CreateRestaurantDto model);
}