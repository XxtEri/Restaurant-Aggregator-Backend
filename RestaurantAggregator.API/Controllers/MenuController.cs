using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("restaurants/{restaurantId}")]
[Produces("application/json")]
public class MenuController: ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly IUserService _userService;

    public MenuController(IMenuService menuService, IUserService userService)
    {
        _menuService = menuService;
        _userService = userService;
    }
    
    /// <summary>
    /// Получить меню ресторана с блюдами
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("menus/{menuId:Guid}")]
    public async Task<ActionResult<MenuModel>> GetMenu(Guid restaurantId, Guid menuId)
    {
        var menuDto = await _menuService.GetMenuDto(restaurantId, menuId);
        
        var dishes = menuDto.Dishes.Select(dish => new DishModel
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Description = dish.Description,
                IsVegetarian = dish.IsVegetarian,
                Photo = dish.Photo,
                Rating = dish.Rating,
                Category = dish.Category
            })
            .ToList();

        return Ok(new MenuModel
        {
            Id = menuDto.Id,
            Name = menuDto.Name,
            Dishes = dishes
        });
    }
    
    /// <summary>
    /// Добавить новое меню для ресторана
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("menus")]
    [Authorize(Roles = UserRoles.Manager)]
    public async Task<ActionResult<MenuModel>> AddMenu(Guid restaurantId, CreateMenuModel model)
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
        
        await _menuService.AddMenuToRestaurant(new Guid(userId), restaurantId, new CreateMenuDto
        {
            Name = model.Name
        });
        
        return Ok();
    }
    
    /// <summary>
    /// Удалить меню из ресторана
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("menus/{menuId:Guid}")]
    [Authorize(Roles = UserRoles.Manager)]
    public async Task<ActionResult<MenuModel>> DeleteMenu(Guid restaurantId, Guid menuId)
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
        
        await _menuService.DeleteMenuFromRestaurant(new Guid(userId), restaurantId, menuId);
        
        return Ok();
    }
}