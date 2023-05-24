using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("basket")]
[Produces("application/json")]
public class CartController: ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;

    public CartController(ICartService cartService, IUserService userService)
    {
        _cartService = cartService;
        _userService = userService;
    }
    
    /// <summary>
    /// Получение списка блюд в корзине пользователя
    /// </summary>
    [HttpGet("dishes")]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(typeof(List<DishInCartModel>), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<DishInCartModel>>> GetListAllDishes()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var dishesInCartDto = await _cartService.GetCartDishes(new Guid(userId));

        var dishesInCartModel = dishesInCartDto
            .Select(o => new DishInCartModel
            {
                Id = o.Id,
                Count = o.Count,
                Dish = getDishModel(o.Dish)
            })
            .ToList();

        return Ok(dishesInCartModel);
    }

    /// <summary>
    /// Получение блюда в корзине пользователя
    /// </summary>
    /// <param name="dishId"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("dishes/{dishId}")]
    public async Task<ActionResult<DishInCartModel>> GetDishInCart(Guid dishId)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);
        
        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var dishInCart = await _cartService.GetDishInCart(new Guid(userId), dishId);

        return new DishInCartModel
        {
            Id = dishInCart.Id,
            Count = dishInCart.Count,
            Dish = getDishModel(dishInCart.Dish)
        };
    }

    /// <summary>
    /// Добавление блюда в корзину
    /// </summary>
    [HttpPost("dishes/{dishId}")]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddDishToBasket(Guid dishId)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        await _cartService.AddDishInCart(new Guid(userId), dishId);

        return Ok();
    }
    /// <summary>
    /// Удаление блюда из корзины
    /// </summary>
    [HttpDelete("dishes/{dishId}")]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDishToBasket(Guid dishId)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        await _cartService.DeleteDishOfCart(new Guid(userId), dishId);

        return new NoContentResult();
    }
    
    /// <summary>
    /// Изменение количества какого-то добавленного в корзину блюда
    /// </summary>
    [HttpPut("dishes/{dishId}/increase")]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeIncreaseDishInCart(Guid dishId, bool increase)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        await _cartService.ChangeQuantity(new Guid(userId), dishId, increase);

        return Ok();
    }
    
    /// <summary>
    /// Полностью очистить корзину
    /// </summary>
    [HttpPost("")]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ClearCart()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        await _cartService.ClearCart(new Guid(userId));

        return Ok();
    }

    private static DishModel getDishModel(DishDTO dishDto)
    {
        return new DishModel
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
    }
}