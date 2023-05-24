using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("restaurants")]
[Produces("application/json")]
public class RestaurantController: ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    /// <summary>
    /// Получение списка ресторанов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(RestaurantPagedListModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RestaurantPagedListModel>> GetListRestaurant([FromQuery] string? nameRestaurant, [DefaultValue(1)] int page)
    {
        var restaurantsDto = await _restaurantService.GetRestaurants(nameRestaurant ?? string.Empty, page);

        var restaurantsModel = new RestaurantPagedListModel
        {
            Restaurants = new List<RestaurantModel>(),
            PageInfoModel = new PageInfoModel(
                restaurantsDto.PageInfoModel.Size, 
                restaurantsDto.PageInfoModel.Count,
                restaurantsDto.PageInfoModel.Current)
        };

        if (restaurantsDto.Restaurants == null) return Ok(restaurantsModel);
        
        foreach (var restaurant in restaurantsDto.Restaurants)
        {
            var restaurantModel = new RestaurantModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
            };

            if (restaurant.Menus != null)
                restaurantModel.Menus = GetMenusOfRestaurant(restaurant.Menus);

            restaurantsModel.Restaurants.Add(restaurantModel);
        }

        return Ok(restaurantsModel);
    }
    
    /// <summary>
    /// Получение информации о конкретном ресторане
    /// </summary>
    [ProducesResponseType(typeof(RestaurantModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    [HttpGet("{restaurantId}")]
    public async Task<ActionResult<RestaurantModel>> GetInformationConcreteRestaurant(Guid restaurantId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var restaurantDto = await _restaurantService.GetRestaurant(restaurantId);

        var restaurantModel = new RestaurantModel
        {
            Id = restaurantDto.Id,
            Name = restaurantDto.Name
        };

        if (restaurantDto.Menus != null)
            restaurantModel.Menus = GetMenusOfRestaurant(restaurantDto.Menus);
        
        return Ok(restaurantModel);
    }

    private static List<MenuModel> GetMenusOfRestaurant(IEnumerable<MenuDTO> menus)
    {
        return menus.Select(menu => new MenuModel { Id = menu.Id, Name = menu.Name }).ToList();
    }
}