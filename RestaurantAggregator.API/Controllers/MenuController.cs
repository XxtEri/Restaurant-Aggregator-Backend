using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("restaurant/{restaurantId}")]
[Produces("application/json")]
public class MenuController: ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }
    
    /// <summary>
    /// Добавить новое меню для ресторана
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("menu")]
    [Authorize(Roles = UserRoles.Manager)]
    public async Task<ActionResult<MenuModel>> AddMenu(Guid restaurantId, CreateMenuModel model)
    {
        var menuDto = await _menuService.AddMenuToRestaurant(restaurantId, new CreateMenuDto
        {
            Name = model.Name
        });
        
        return Ok(new MenuModel
        {
            Id = menuDto.Id,
            Name = menuDto.Name
        });
    }

}