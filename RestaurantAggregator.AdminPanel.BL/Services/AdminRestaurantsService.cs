using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.BL.Services;

public class AdminRestaurantsService: IAdminRestaurantsService
{
    private readonly IRestaurantService _restaurantService;
    
    public AdminRestaurantsService(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    public async Task Create(CreateRestaurantDto model)
    {
        await _restaurantService.CreateRestaurant(model);
    }

    public async Task<RestaurantDTO> Get(Guid id)
    {
        return await _restaurantService.GetRestaurant(id);
    }
    
    public async Task<List<RestaurantDTO>> Select(string? searchingName)
    {
        try
        {
            var r = await _restaurantService.GetRestaurants(searchingName ?? string.Empty, 1);

            return r.Restaurants;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task Update(Guid id, RestaurantDTO model)
    {
        try
        {
            await _restaurantService.UpdateRestaurant(id, new UpdateInfoRestaurantDto
            {
                Name = model.Name
            });
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task Delete(Guid id)
    {
        try
        {
            await _restaurantService.DeleteRestaurant(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}