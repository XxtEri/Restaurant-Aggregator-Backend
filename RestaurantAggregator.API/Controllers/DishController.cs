using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;


[ApiController]
[Route("")]
[Produces("application/json")]
public class DishController
{
    /// <summary>
    /// Получение списка всех блюд в ресторане
    /// </summary>
    [HttpGet("restaurants/{restaurantId}/dishes")]
    [ProducesResponseType(typeof(DishPagedListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public string GetListAllDishes(Guid restaurantId)
    {
        return "";
    }
    
    /// <summary>
    /// Получение списка блюд в конкретном меню ресторана
    /// </summary>
    [HttpGet("restaurant/{restaurantId}/menu/{menuId}/dishes")]
    [ProducesResponseType(typeof(DishPagedListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public string GetListDishesInMenu(Guid restaurantId, Guid menuId)
    {
        return "";
    }
    
    /// <summary>
    /// Получение информации о конкретном блюде
    /// </summary>
    [ProducesResponseType(typeof(RestaurantDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    [HttpGet("dishes/{dishId}")]
    public string GetInformationConcreteRestaurant(Guid dishId)
    {
        return "";
    }

    /// <summary>
    /// Проверка на то, можно ли пользователь поставить оценку конкретному блюду
    /// </summary>
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    [HttpGet("dishes/{dishId}/check")]
    [Authorize]
    public string CheckCurrentUserSetRatingToDish()
    {
        return "";
    }
    
    /// <summary>
    /// Поставить рейтинг блюду
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    [HttpPost("dishes/{dishId}/rating")]
    [Authorize]
    public string SetRatingToDish(Guid dishId, [Range(0,10)] int ratingScore)
    {
        return "";
    }
}