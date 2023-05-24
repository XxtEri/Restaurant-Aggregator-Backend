using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.CommonFiles.Enums;
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
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RestaurantPagedListModel>> GetListRestaurant([FromQuery] string? nameRestaurant, [DefaultValue(1)] int page)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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
                Name = restaurant.Name
            };

            if (restaurant.Menus.Any())
                restaurantModel.Menus = GetMenusOfRestaurant(restaurant.Menus);

            restaurantsModel.Restaurants.Add(restaurantModel);
        }

        return Ok(restaurantsModel);
    }
    
    /// <summary>
    /// Получение информации о конкретном ресторане
    /// </summary>
    [ProducesResponseType(typeof(RestaurantModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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

    private static List<MenuModel> GetMenusOfRestaurant(List<MenuDTO> menus)
    {
        var menusModel = new List<MenuModel>();
        foreach (var menu in menus)
        {
            var menuModel = new MenuModel
            {
                Id = menu.Id,
                Name = menu.Name
            };

            foreach (var dish in menu.Dishes)
            {
                var dishModel = new DishModel
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Description = dish.Description,
                    IsVegetarian = dish.IsVegetarian,
                    Photo = dish.Photo,
                    Rating = dish.Rating,
                    Category = dish.Category
                };
                
                menuModel.Dishes.Add(dishModel);
            }
            
            menusModel.Add(menuModel);
        }

        return menusModel;
    }
}