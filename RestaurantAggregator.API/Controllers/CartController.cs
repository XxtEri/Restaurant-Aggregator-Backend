using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.BL.Services;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("basket")]
[Produces("application/json")]
public class CartController
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    
    /// <summary>
    /// Получение списка блюд в корзине пользователя
    /// </summary>
    [HttpGet("dishes")]
    [Authorize]
    [ProducesResponseType(typeof(DishPagedListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public string GetListAllDishes(Guid restaurantId)
    {
        return "";
    }
    
    /// <summary>
    /// Добавление блюда в корзину
    /// </summary>
    [HttpPost("dishes/{dishId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public string AddDishToBasket(Guid dishId)
    {
        return "";
    }
    
    /// <summary>
    /// Удаление блюда в корзину
    /// </summary>
    [HttpDelete("dishes/{dishId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public string DeleteDishToBasket(Guid dishId)
    {
        return "";
    }
    
    /// <summary>
    /// Удаление блюда в корзину
    /// </summary>
    [HttpPost("dishes/{dishId}/increase")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public string ChangeIncreaseDishInBasket(Guid dishId, bool increase)
    {
        return "";
    }
}