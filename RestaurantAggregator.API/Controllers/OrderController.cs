using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("")]
[Produces("application/json")]
public class OrderController
{
    public OrderController()
    {
        
    }
    
    /// <summary>
    /// Получение списка прошлых заказов пользователя с ролью Client
    /// </summary>
    [HttpGet("customers/orders/last")]
    [ProducesResponseType(typeof(OrderPageListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [Authorize]
    public string GetListLastOrderForClient()
    {
        return "";
    }
    
    /// <summary>
    /// Получение списка текущих заказов пользователя с ролью Client
    /// </summary>
    [ProducesResponseType(typeof(OrderPageListDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [HttpGet("customers/orders/active")]
    [Authorize]
    public string GetActiveOrderForCourier()
    {
        return "";
    }
    
    /// <summary>
    /// Получение истории заказов, приготовленных пользователем с ролью Cook
    /// </summary>
    [HttpGet("/cooks/last-orders")]
    [Authorize]
    [ProducesResponseType(typeof(OrderPageListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public string GetListLastOrderForCook()
    {
        return "";
    }
    
    /// <summary>
    /// Получение списка всех заказов со статусом Created для пользователя роли Cook
    /// </summary>
    [ProducesResponseType(typeof(OrderPageListDTO), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [HttpGet("restaurant/{restaurantId}/cookies/orders")]
    [Authorize]
    public string GetListActiveOrderForCook(Guid restaurantId)
    {
        return "";
    }

    /// <summary>
    /// Получение списка всех заказов в ресторане для пользователя роли Manager
    /// </summary>
    [ProducesResponseType(typeof(OrderPageListDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [HttpGet("restaurant/{restaurantId}/managers/orders")]
    [Authorize]
    public string GetListOrderForManager(Guid restaurantId)
    {
        return "";
    }
    
    /// <summary>
    /// Получение списка всех заказов для пользователя роли Courier
    /// </summary>
    [HttpGet("couriers/orders")]
    [Authorize]
    [ProducesResponseType(typeof(OrderPageListDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public string GetListOrderForCourier()
    {
        return "";
    }
    
    /// <summary>
    /// Создание нового заказа для пользователя роли Courier
    /// </summary>
    [HttpPost("customers/orders")]
    [Authorize]
    [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public string CreateNewOrder([FromBody] OrderCreateDTO model)
    {
        return "";
    }
    
    /// <summary>
    /// Повторение прошлого заказа для пользователя роли Courier
    /// </summary>
    [HttpPost("customers/orders/{orderId}/repeat")]
    [Authorize]
    [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public string RepeatLastOrder(Guid orderId, [FromBody] OrderCreateDTO model)
    {
        return "";
    }
    
    /// <summary>
    /// Изменение статуса заказа
    /// (для роли Client меняется статус только с “Created” на “Canceled”)
    /// (для роли Cook меняется статус на “Kitchen”, “Packaging” и “Waiting Courier”)
    /// (для роли Courier меняется статус на “Delivered” и на “Canceled” только со статуса “Delivery”)
    /// </summary>
    [HttpPost("orders/{order-id}/status")]
    [Authorize]
    [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public string ChangeOrderStatus(Guid orderId, [FromQuery] OrderStatus status)
    {
        return "";
    }
}