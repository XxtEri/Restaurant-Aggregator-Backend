using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.CommonFiles;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregatorService.Models;

namespace RestaurantAggregatorService.Controllers;

[ApiController]
[Route("")]
[Produces("application/json")]
public class OrderController: ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    
    public OrderController(IOrderService orderService, IUserService userService)
    {
        _orderService = orderService;
        _userService = userService;
    }
    
    /// <summary>
    /// Получение списка прошлых заказов пользователя с ролью Customer
    /// </summary>
    [HttpGet("customers/orders/last")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(OrderPageListModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderPageListModel>> GetListLastOrderForCustomer([DefaultValue(1)] int page, DateTime? startDay, DateTime? endDay)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var orders = await _orderService.GetListLastOrderForCustomer(new Guid(userId), page, startDay, endDay);
        var ordersModel = orders.Orders!
            .Select(order => new OrderModel
            {
                Id = order.Id,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToList();

        return Ok(ordersModel);
    }
    
    /// <summary>
    /// Получение списка текущих заказов пользователя с ролью Customer
    /// </summary>
    [HttpGet("customers/orders/active")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType( StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderModel>>> GetActiveOrderForCustomer()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var ordersDto = await _orderService.GetListActiveOrderForCustomer(new Guid(userId));

        var ordersModel = ordersDto.Select(orderDto => new OrderModel
            {
                Id = orderDto.Id,
                NumberOrder = orderDto.NumberOrder,
                DeliveryTime = orderDto.DeliveryTime,
                OrderTime = orderDto.OrderTime,
                Price = orderDto.Price,
                Address = orderDto.Address,
                Status = orderDto.Status
            })
            .ToList();

        return Ok(ordersModel);
    }
    
    /// <summary>
    /// Получение информации о конкретном заказе пользователя с ролью Customer
    /// </summary>
    [HttpGet("customers/orders/{numberOrder:long}")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderModel>> GetOrder(long numberOrder)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        var orderDto = await _orderService.GetConcreteOrder(new Guid(userId), numberOrder);

        var orderModel = new OrderModel
        {
            Id = orderDto.Id,
            NumberOrder = orderDto.NumberOrder,
            DeliveryTime = orderDto.DeliveryTime,
            OrderTime = orderDto.OrderTime,
            Price = orderDto.Price,
            Address = orderDto.Address,
            Status = orderDto.Status
        };

        return Ok(orderModel);
    }
    
    /// <summary>
    /// Создание нового заказа для пользователя с ролью Customer
    /// </summary>
    [HttpPost("customers/orders")]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateNewOrder([FromBody] OrderCreateModel model)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        await _orderService.CreateNewOrder(new Guid(userId), new OrderCreateDTO
        {
            DeliveryTime = model.DeliveryTime,
            Address = model.Address
        });
        
        return Ok();
    }

    /// <summary>
    /// Повторение прошлого заказа для пользователя роли Customer
    /// </summary>
    [HttpPost("customers/orders/{numberOrder:long}/repeat")]
    [Authorize(Roles = UserRoles.Customer)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RepeatLastOrder(long numberOrder, [FromBody] OrderCreateModel model)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        await _orderService.RepeatLastOrder(new Guid(userId), numberOrder, new OrderCreateDTO
        {
            DeliveryTime = model.DeliveryTime,
            Address = model.Address
        });

        return Ok();
    }

    
    
    /// <summary>
    /// Получение списка всех заказов, доступных для доставки, для пользователя с ролью Courier
    /// </summary>
    [HttpGet("couriers/orders")]
    [Authorize(Roles = UserRoles.Courier)]
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderModel>>> GetListOrderForCourier()
    {
        var ordersDto = await _orderService.GetOrdersForDelivery();

        var ordersModel = ordersDto
            .Select(o => new OrderModel
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            });
        
        return Ok(ordersModel);
    }
    
    /// <summary>
    /// Получение списка активного заказа для пользователя с ролью Courier
    /// </summary>
    [HttpGet("couriers/order/active")]
    [Authorize(Roles = UserRoles.Courier)]
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderModel>>> GetActiveOrderForCourier()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var ordersDto = await _orderService.GetActiveOrdersForCourier(new Guid(userId));

        var ordersModel = ordersDto
            .Select(o => new OrderModel
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            });
        
        return Ok(ordersModel);
    }
    
    /// <summary>
    /// Получение списка всех доставленных заказов пользователем с ролью Courier
    /// </summary>
    [HttpGet("couriers/orders/last")]
    [Authorize(Roles = UserRoles.Courier)]
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderModel>>> GetLastOrdersForCourier()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var ordersDto = await _orderService.GetLastOrderForCourier(new Guid(userId));

        var ordersModel = ordersDto
            .Select(o => new OrderModel
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            });
        
        return Ok(ordersModel);
    }

    
    
    /// <summary>
    /// Получение истории заказов, приготовленных пользователем с ролью Cook
    /// </summary>
    [HttpGet("/cooks/orders/last")]
    [Authorize(Roles = UserRoles.Cook)]
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderModel>>> GetListLastOrderForCook()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var ordersDto = await _orderService.GetListLastOrderForCook(new Guid(userId));

        var ordersModel = ordersDto
            .Select(o => new OrderModel
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            });
        
        return Ok(ordersModel);
    }
    
    /// <summary>
    /// Получение заказов, готовые для приготовления, для пользователя с ролью Cook
    /// </summary>
    [HttpGet("/cooks/orders")]
    [Authorize]
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderModel>>> GetListOrderForCooking()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var ordersDto = await _orderService.GetListOrderForCook(new Guid(userId));

        var ordersModel = ordersDto
            .Select(o => new OrderModel
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            });
        
        return Ok(ordersModel);
    }

    /// <summary>
    /// Получение заказов, взятых для готовки пользователем с ролью Cook
    /// </summary>
    [HttpGet("/cooks/orders/active")]
    [Authorize(Roles = UserRoles.Cook)]
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OrderModel>>> GetListActiveOrderForCooking()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var ordersDto = await _orderService.GetListActiveOrderForCook(new Guid(userId));

        var ordersModel = ordersDto
            .Select(o => new OrderModel
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            });
        
        return Ok(ordersModel);
    }

    

    /// <summary>
    /// Получение списка всех заказов в ресторане для пользователя с ролью Manager
    /// </summary>
    [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("restaurant/{restaurantId}/managers/orders")]
    [Authorize(Roles = UserRoles.Manager)]
    public async Task<ActionResult<List<OrderModel>>> GetListOrderForManager(
        int page,
        OrderStatus? status,
        DateTime? startOrderTime, 
        DateTime? endOrderTime,
        DateTime? startDeliveryTime, 
        DateTime? endDeliveryTime,
        long? numberOrder)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }
        
        var ordersDto = await _orderService.GetListOrderForManager(
            new Guid(userId),
            page,
            status,
            startOrderTime, endOrderTime,
            startDeliveryTime, endDeliveryTime,
            numberOrder
        );

        var ordersModel = ordersDto
            .Select(o => new OrderModel
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            });
        
        return Ok(ordersModel);
    }

    /// <summary>
    /// Изменение статуса заказа
    /// (для роли Client меняется статус только с “Created” на “Canceled”)
    /// (для роли Cook меняется статус на “Kitchen”, “Packaging” и “Waiting Courier”)
    /// (для роли Courier меняется статус на “Delivered” и на “Canceled” только со статуса “Delivery”)
    /// </summary>
    [HttpPost("orders/{order-id}/status")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeOrderStatus(Guid orderId, [FromQuery] OrderStatus status)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = _userService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return StatusCode(500, "Возникла ошибка при парсинге токена");
        }

        await _orderService.ChangeOrderStatus(new Guid(userId), orderId, status);
        
        return Ok();
    }
}