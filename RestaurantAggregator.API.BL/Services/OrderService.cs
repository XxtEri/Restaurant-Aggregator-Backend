using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class OrderService: IOrderService
{
    private readonly ApplicationDBContext _context;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;

    public OrderService(ApplicationDBContext context, IUserService userService, ICartService cartService)
    {
        _context = context;
        _userService = userService;
        _cartService = cartService;
    }
    
    public async Task<OrderPageListDTO> GetListLastOrder(
        Guid userId, 
        int page, 
        DateTime? startDay, 
        DateTime? endDay)
    {
        if (page < 1)
        {
            throw new NotCorrectDataException(message: "Page value must be greater than 0");
        }
        
        var orders = await _context.Orders
            .Where(order => order.CustomerId == userId && order.Status == OrderStatus.Delivered)
            .Select(order => new OrderDTO
            {
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();
        
        if (!orders.Any())
        {
            throw new NotFoundException("У вас пока еще нет завершенных заказов");
        }

        if (startDay != null && endDay == null)
        {
            orders = orders
                .Where(order => order.DeliveryTime >= startDay)
                .ToList();
        }

        if (startDay == null && endDay != null)
        {
            orders = orders
                .Where(order => order.DeliveryTime <= endDay)
                .ToList();
        }

        const int pageSize = 5;
        var countDishes = orders.Count;
        var count = countDishes % pageSize < pageSize && countDishes % pageSize != 0 ? countDishes / 5 + 1 : countDishes / 5;

        if (page > count)
        {
            throw new NotCorrectDataException(message: "Invalid value for attribute page");
        }

        var itemsOrder = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new OrderPageListDTO()
        {
            Orders = itemsOrder,
            PageInfoModel = new PageInfoModelDTO(pageSize, count, page)
        };
    }

    public async Task<OrderDTO> GetConcreteOrder(Guid userId, string numberOrder)
    {
        var order = await _context.Orders
            .Where(order => order.NumberOrder == numberOrder)
            .Select(order => new OrderDTO
            {
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .FirstOrDefaultAsync();

        if (order == null)
        {
            throw new NotFoundException($"Заказа по номеру {numberOrder} не найдено");
        }
        
        return order;
    }

    public async Task CreateNewOrder(Guid userId, OrderCreateDTO model)
    {
        if (model.DeliveryTime <= DateTime.UtcNow.AddHours(1))
        {
            throw new NotCorrectDataException( message: "Invalid delivery time. Delivery time must be more than current datetime on 60 minutes");
        }

        //var dishesInCart = await _cartService.GetCartDishes(userId);
        var dishesInCart = new System.Collections.Generic.List<DishInCartDto>();
        
        if (dishesInCart.Count == 0)
        {
            throw new NotFoundException(message: $"Невозможно создать новый заказ, так как корзина пока еще пуста у пользователя с id={userId}");
        }

        var customer = await _context.Customers
            .Where(c => c.Id == userId)
            .FirstOrDefaultAsync();

        if (customer == null)
            customer.Id = await _userService.AddNewCustomerToDb(userId);
        
        var order = new Order
        {
            DeliveryTime = model.DeliveryTime,
            OrderTime = DateTime.Now,
            Address = model.Address,
            Status = OrderStatus.Created,
            Price = SumPriceDishes(dishesInCart),
            Customer = customer
        };
        
        _context.Orders.Add(order);

        //await _cartService.ClearCart(userId);
        foreach (var dishInCart in dishesInCart)
        {
            _context.OrdersDishes.Add(new OrderDish
            {
                Order = order,
                Dish = await _context.Dishes.FindAsync(dishInCart.Dish.Id)
            });
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task RepeatLastOrder(Guid orderId, OrderCreateDTO model)
    {
        var order = await _context.Orders.LastAsync();

        await _context.AddAsync(new Order
        {
            NumberOrder = "",
            DeliveryTime = model.DeliveryTime,
            Price = order.Price,
            Status = OrderStatus.Created,
            Address = model.Address
        });
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderDTO>> GetActiveOrderForCourier(Guid userId)
    {
        var orders = await _context.Orders
            .Where(order => order.CustomerId == userId && order.Status != OrderStatus.Delivered)
            .Select(order => new OrderDTO
            {
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();
    
        if (!orders.Any())
        {
            throw new NotFoundElementException("У вас пока еще нет активных заказов");
        }
        
        return orders;
    }

    public Task<List<OrderDTO>> GetListOrderForCourier()
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDTO>> GetListLastOrderForCook()
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDTO>> GetListActiveOrderForCook(Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDTO>> GetListOrderForManager(Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task ChangeOrderStatus(Guid orderId, OrderStatus status)
    {
        throw new NotImplementedException();
    }
    
    private static double SumPriceDishes(IEnumerable<DishInCartDto> dishes)
    {
        return dishes.Sum(dish => dish.Count * dish.Dish.Price);
    }
}