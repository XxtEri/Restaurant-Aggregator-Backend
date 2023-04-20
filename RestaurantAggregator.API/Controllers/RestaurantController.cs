using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("restaurants")]
[Produces("application/json")]
public class RestaurantController: ControllerBase
{
    public RestaurantController()
    {
        
    }

    /// <summary>
    /// Получение списка ресторанов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(RestaurantPagedListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public string GetListRestaurant()
    {
        return "";
    }
    
    /// <summary>
    /// Получение информации о конкретном ресторане
    /// </summary>
    [ProducesResponseType(typeof(RestaurantDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public string GetInformationConcreteRestaurant(Guid id)
    {
        return "";
    }
}