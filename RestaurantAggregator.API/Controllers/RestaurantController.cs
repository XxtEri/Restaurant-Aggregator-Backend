using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
    [ProducesResponseType(typeof(RestaurantPagedListDto), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RestaurantPagedListDto>> GetListRestaurant([FromQuery] string? nameRestaurant, [DefaultValue(1)] int page)
    {
        var restaurants = await _restaurantService.GetRestaurants(nameRestaurant ?? string.Empty, page);

        return Ok(restaurants);
    }
    
    /// <summary>
    /// Получение информации о конкретном ресторане
    /// </summary>
    [ProducesResponseType(typeof(RestaurantDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public string GetInformationConcreteRestaurant(Guid id)
    {
        return "";
    }
}