using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.CommonFiles.Exceptions;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;


[ApiController]
[Route("")]
[Produces("application/json")]
public class DishController: ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

        /// <summary>
    /// Получение списка всех блюд в ресторане
    /// </summary>
    [HttpGet("restaurants/{restaurantId}/dishes")]
    [ProducesResponseType(typeof(DishPagedListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DishPagedListDTO>> GetListAllDishesInRestaurant(Guid restaurantId, 
            [FromQuery] List<DishCategory> categories, 
            [DefaultValue(false)] bool vegetarian,
            SortingDish sorting,
            [DefaultValue(1)] int page)
    {
        try
        {
            var dishes =
                await _dishService.GetListAllDishesInRestaurant(restaurantId, categories, vegetarian, sorting, page);
            return Ok(dishes);
        }
        catch (NotCorrectDataException e)
        {
            return BadRequest(new ResponseModel
            {
                Status = "400 error",
                Message = e.Message
            });
        }
        catch (NotFoundElementException e)
        {
            return NotFound(new ResponseModel
            {
                Status = "404 error",
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResponseModel
            {
                Status = "500 error",
                Message = e.Message
            });
        }
    }
    
    /// <summary>
    /// Получение списка блюд в конкретном меню ресторана
    /// </summary>
    [HttpGet("restaurant/{restaurantId}/menu/{menuId}/dishes")]
    [ProducesResponseType(typeof(DishPagedListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetListDishesInMenu(Guid restaurantId, 
        Guid menuId,
        [FromQuery] List<DishCategory> categories, 
        [DefaultValue(false)] bool vegetarian,
        SortingDish sorting,
        [DefaultValue(1)] int page)
    {
        try
        {
            var dishes =
                await _dishService.GetListDishesInMenu(restaurantId, menuId, categories, vegetarian, sorting, page);
            return Ok(dishes);
        }
        catch (NotCorrectDataException e)
        {
            return BadRequest(new ResponseModel
            {
                Status = "400 error",
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResponseModel
            {
                Status = "500 error",
                Message = e.Message
            });
        }
    }
    
    /// <summary>
    /// Получение информации о конкретном блюде
    /// </summary>
    [ProducesResponseType(typeof(RestaurantDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    [HttpGet("dishes/{dishId}")]
    public async Task<IActionResult> GetInformationConcreteDish(Guid dishId)
    {
        try
        {
            var dish = await _dishService.GetDishInformation(dishId);
            return Ok(dish);
        }
        catch (NotFoundElementException e)
        {
            return NotFound(new ResponseModel
            {
                Status = "404 error",
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResponseModel
            {
                Status = "500 error",
                Message = e.Message
            });
        }
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