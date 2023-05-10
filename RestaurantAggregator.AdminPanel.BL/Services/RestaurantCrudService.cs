using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.AuthApi.Common.Exceptions;

namespace RestaurantAggregator.AdminPanel.BL.Services;

public class RestaurantCrudService: IRestaurantCrudService
{
    private readonly IRestaurantService _restaurantService;
    
    public RestaurantCrudService(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    public async Task Create(RestaurantDTO model)
    {
        await _restaurantService.CreateRestaurant(model);
    }

    public async Task<RestaurantDTO> Get(Guid id)
    {
        return await _restaurantService.GetRestaurant(id);
    }
    
    public async Task<List<RestaurantDTO>> Select()
    {
        try
        {
            return await _restaurantService.GetRestaurants();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task Update(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}