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

    public bool Create(RestaurantDTO entity)
    {
        throw new NotImplementedException();
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

    public bool Update(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}