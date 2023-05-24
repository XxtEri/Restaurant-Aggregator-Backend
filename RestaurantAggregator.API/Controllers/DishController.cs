using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Net.Http.Headers;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;


[ApiController]
[Route("")]
[Produces("application/json")]
public class DishController: ControllerBase
{
    private readonly IDishService _dishService;
    private readonly IUserService _userService;

    public DishController(IDishService dishService, IUserService userService)
    {
        _dishService = dishService;
        _userService = userService;
    }

    /// <summary>
    /// Получение списка всех блюд в ресторане
    /// </summary>
    [HttpGet("restaurants/{restaurantId}/dishes")]
    [ProducesResponseType(typeof(DishPagedListModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DishPagedListModel>> GetListAllDishesInRestaurant(
        Guid restaurantId, 
        [FromQuery] List<DishCategory> categories, 
        [DefaultValue(false)] bool vegetarian, 
        SortingDish sorting, 
        [DefaultValue(1)] int page)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var dishesDto =
            await _dishService.GetListAllDishesInRestaurant(restaurantId, categories, vegetarian, sorting, page);
        var dishesModel = GetDishPagedListModel(dishesDto);

        return Ok(dishesModel);
    }
    
    /// <summary>
    /// Получение списка блюд в конкретном меню ресторана
    /// </summary>
    [HttpGet("restaurants/{restaurantId}/menus/{menuId}/dishes")]
    [ProducesResponseType(typeof(DishPagedListModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DishPagedListModel>> GetListDishesInMenu(Guid restaurantId, 
        Guid menuId,
        [DefaultValue(1)] int page)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var dishesDto =
            await _dishService.GetListDishesInMenu(restaurantId, menuId, page);
        var dishesModel = GetDishPagedListModel(dishesDto);

        return Ok(dishesModel);
    }
    
    /// <summary>
    /// Получение информации о конкретном блюде
    /// </summary>
    [ProducesResponseType(typeof(DishModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("dishes/{dishId}")]
    public async Task<ActionResult<DishModel>> GetInformationConcreteDish(Guid dishId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var dishDto = await _dishService.GetDishInformation(dishId);
        var dishModel = new DishModel
        {
            Id = dishDto.Id,
            Name = dishDto.Name,
            Price = dishDto.Price,
            Description = dishDto.Description,
            IsVegetarian = dishDto.IsVegetarian,
            Photo = dishDto.Photo,
            Rating = dishDto.Rating,
            Category = dishDto.Category
        };
        
        return Ok(dishModel);
    }

    /// <summary>
    /// Проверка на то, можно ли пользователь поставить оценку конкретному блюду
    /// </summary>
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("dishes/{dishId}/check")]
    [Authorize(Roles = $"{UserRoles.Customer}, {UserRoles.Manager}")]
    public async Task<ActionResult<bool>> CheckCurrentUserSetRatingToDish(Guid dishId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var check = await _dishService.CheckCurrentUserSetRatingToDish(new Guid(userId), dishId);
        
        return Ok(check);
    }
    
    /// <summary>
    /// Поставить рейтинг блюду
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost("dishes/{dishId}/rating")]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> SetRatingToDish(Guid dishId, [Range(0,10)] int ratingScore)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        await _dishService.SetRatingToDish(new Guid(userId), dishId, ratingScore);

        return Ok();
    }

    /// <summary>
    /// Добавить блюдо в меню ресторана
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost("restaurants/{restaurantId}/menus/{menuId}/dishes")]
    [Authorize(Roles = UserRoles.Manager)]
    public async Task<IActionResult> AddDishToMenuOfRestaurant(Guid restaurantId, Guid menuId, CreateDishModel model)
    {
        await _dishService.AddDishToMenuOfRestaurant(restaurantId, menuId, new CreateDishDto
        {
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            IsVegetarian = model.IsVegetarian,
            Photo = model.Photo,
            Category = model.Category
        });
        
        return Ok();
    }
    
    private static DishPagedListModel GetDishPagedListModel(DishPagedListDTO dishPagedListDto)
    {
        var dishes = new DishPagedListModel
        {
            Dishes = new List<DishModel>(),
            PageInfoModel = new PageInfoModel(
                dishPagedListDto.PageInfoModel.Size, 
                dishPagedListDto.PageInfoModel.Count,
                dishPagedListDto.PageInfoModel.Current)
        };

        if (dishPagedListDto.Dishes == null) return dishes;
        
        dishes.Dishes = dishPagedListDto.Dishes.Select(dish => new DishModel
        {
            Id = dish.Id,
            Name = dish.Name,
            Price = dish.Price,
            Description = dish.Description,
            IsVegetarian = dish.IsVegetarian,
            Photo = dish.Photo,
            Rating = dish.Rating,
            Category = dish.Category
        }).ToList();

        return dishes;
    }
}