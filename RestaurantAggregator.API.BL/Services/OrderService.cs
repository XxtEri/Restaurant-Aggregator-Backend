using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.CommonFiles.Enums;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class OrderService: IOrderService
{
    private readonly ApplicationDBContext _context;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;
    private readonly IProducerService _producerService;

    public OrderService(ApplicationDBContext context, IUserService userService, ICartService cartService, IProducerService producerService)
    {
        _context = context;
        _userService = userService;
        _cartService = cartService;
        _producerService = producerService;
    }
    
    public async Task<OrderPageListDTO> GetListLastOrderForCustomer (
        Guid userId, 
        int page, 
        int? numberOrder,
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
                Id = order.Id,
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();
        
        if (numberOrder != null)
        {
            orders = orders.Where(r => r.NumberOrder.ToString().ToLower().Contains(numberOrder?.ToString().Trim().ToLower() ?? string.Empty)).ToList();
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
        
        if (startDay != null && endDay != null)
        {
            orders = orders
                .Where(order => order.DeliveryTime <= endDay && order.DeliveryTime >= startDay)
                .ToList();
        }

        const int pageSize = 5;
        var countDishes = orders.Count;
        var count = countDishes % pageSize < pageSize && countDishes % pageSize != 0 ? countDishes / 5 + 1 : countDishes / 5;

        if (page > count && orders.Any())
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
    
    public async Task<List<OrderDTO>> GetListActiveOrderForCustomer (Guid userId)
    {
        var orders = await _context.Orders
            .Where(order => order.CustomerId == userId && order.Status != OrderStatus.Delivered)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();

        return orders;
    }
    
    public async Task<OrderDTO> GetConcreteOrder(Guid userId, long numberOrder)
    {
        var order = await _context.Orders
            .Where(order => order.NumberOrder == numberOrder)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
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
        
        model.DeliveryTime = model.DeliveryTime.ToUniversalTime();

        var dishesInCart = await _context.DishesInCart
            .Where(d => d.CustomerId == userId)
            .ToListAsync();

        if (dishesInCart.Count == 0)
        {
            throw new NotFoundException(message: $"Невозможно создать новый заказ, так как корзина пока еще пуста у пользователя с id={userId}");
        }

        if (!await _cartService.CheckDishesFromSameRestaurant(userId))
        {
            throw new InvalidResponseException("Невозможно создать заказ, так как в корзине добавлены блюда из разных ресторанов");
        }

        var customer = await _context.Customers
            .FindAsync(userId);

        if (customer == null)
        {
            var customerId = await _userService.AddNewCustomerToDb(userId);
            customer = await _context.Customers
                .FindAsync(customerId);
        }

        if (customer == null)
        {
            throw new NotFoundException($"Не найден покупатель с id = {userId}");
        }

        var order = new Order
        {
            DeliveryTime = model.DeliveryTime,
            OrderTime = DateTime.UtcNow,
            Price = await SumPriceDishes(dishesInCart),
            Address = model.Address,
            Customer = customer,
            Status = OrderStatus.Created
        };

        await _context.Orders.AddAsync(order);

        await _cartService.ClearCart(userId);
        foreach (var dishInCart in dishesInCart)
        {
            var dish = await _context.Dishes.FindAsync(dishInCart.DishId);

            if (dish == null)
            {
                Console.WriteLine("Произошла ошибка с БД блюд, почему-то блюдо отсутствует, хотя было в корзине");
                continue;
            }
            
            await _context.OrdersDishes.AddAsync(new OrderDish
            {
                Order = order,
                Dish = dish
            });
        }
        
        await _context.SaveChangesAsync();
    }
    
    public async Task RepeatLastOrder(Guid userId, long numberOrder, OrderCreateDTO model)
    {
        var lastOrder = await _context.Orders.Where(o => o.NumberOrder == numberOrder).FirstOrDefaultAsync();

        if (lastOrder == null)
        {
            throw new NotFoundException($"Не найден заказ с номером заказа = {numberOrder}");
        }

        if (lastOrder.CustomerId != userId)
        {
            throw new ForbiddenException($"У пользователя с id = {userId} нет доступа с заказу с номером заказа = {numberOrder}");
        }

        var orderingDishes = await _context.OrdersDishes
            .Where(o => o.OrderId == lastOrder.Id)
            .ToListAsync();
        
        var customer = await _context.Customers
            .FindAsync(userId);

        if (customer == null)
        {
            var customerId = await _userService.AddNewCustomerToDb(userId);
            customer = await _context.Customers
                .FindAsync(customerId);
        }

        if (customer == null)
        {
            throw new NotFoundException($"Не найден покупатель с id = {userId}");
        }
        
        var newOrder = new Order
        {
            DeliveryTime = model.DeliveryTime,
            OrderTime = DateTime.UtcNow,
            Price = lastOrder.Price,
            Address = model.Address,
            Customer = customer,
            Status = OrderStatus.Created
        };
        await _context.AddAsync(newOrder);
        
        foreach (var orderingDish in orderingDishes)
        {
            var dish = await _context.Dishes.FindAsync(orderingDish.DishId);

            if (dish == null)
            {
                Console.WriteLine("Произошла ошибка с БД блюд, почему-то блюдо отсутствует");
                continue;
            }
            
            await _context.OrdersDishes.AddAsync(new OrderDish
            {
                Order = newOrder,
                Dish = dish
            });
        }
        
        await _context.SaveChangesAsync();
    }

    
    public async Task<List<OrderDTO>> GetOrdersForDelivery()
    {
        return await _context.Orders
            .Where(order => order.Status == OrderStatus.WaitingCourier)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();
    }
    
    public async Task<OrderDTO?> GetActiveOrderForCourier(Guid courierId)
    {
        return await _context.Orders
            .Where(order => order.Status == OrderStatus.Delivery && order.CourierId == courierId)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<OrderDTO>> GetLastOrderForCourier(Guid courierId)
    {
        return await _context.Orders
            .Where(order => order.Status == OrderStatus.Delivered && order.CourierId == courierId)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();
    }

    
    public async Task<List<OrderDTO>> GetListLastOrderForCook(Guid cookId)
    {
        return await _context.Orders
            .Where(order => (order.Status == OrderStatus.WaitingCourier || 
                             order.Status == OrderStatus.Delivery || 
                             order.Status == OrderStatus.Delivered) 
                            && order.CookId == cookId)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();
    }
    
    public async Task<List<OrderDTO>> GetListOrderForCook(Guid cookId)
    {
        var cook = await _context.Cooks
            .Where(c => c.Id == cookId)
            .FirstOrDefaultAsync();

        if (cook == null)
        {
            throw new NotFoundException($"Не найден повар с id = {cookId}");
        }
        
        var orders = await _context.Orders
            .Where(o => o.Status == OrderStatus.Created)
            .Select(o => new OrderDTO
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            })
            .ToListAsync();

        var ordersForCooking = new List<OrderDTO>();
        foreach (var order in orders)
        {
            var dishId = await _context.OrdersDishes
                .Where(o => o.OrderId == order.Id)
                .Select(o => o.DishId)
                .FirstOrDefaultAsync();
            var menuId = await _context.MenusDishes
                .Where(o => o.DishId == dishId)
                .Select(o => o.MenuId)
                .FirstOrDefaultAsync();
            var restaurantId = await _context.Menus
                .Where(m => m.Id == menuId)
                .Select(m => m.RestaurantId)
                .FirstOrDefaultAsync();

            if (restaurantId == cook.RestaurantId)
            {
                ordersForCooking.Add(order);
            }
        }

        return ordersForCooking;
    }
    
    public async Task<List<OrderDTO>> GetListActiveOrderForCook(Guid cookId)
    {
        var cook = await _context.Cooks
            .Where(c => c.Id == cookId)
            .FirstOrDefaultAsync();

        if (cook == null)
        {
            throw new NotFoundException($"Не найден повар с id = {cookId}");
        }
        
        var orders = await _context.Orders
            .Where(o => (o.Status == OrderStatus.Kitchen || 
                         o.Status == OrderStatus.Packaging) 
                        && o.CookId == cookId)
            .Select(o => new OrderDTO
            {
                Id = o.Id,
                NumberOrder = o.NumberOrder,
                DeliveryTime = o.DeliveryTime,
                OrderTime = o.OrderTime,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status
            })
            .ToListAsync();

        return orders;
    }
    
    
    public async Task<List<OrderDTO>> GetListOrderForManager(
        Guid managerId,
        int page,
        OrderStatus? status,
        DateTime? startOrderTime, 
        DateTime? endOrderTime,
        DateTime? startDeliveryTime, 
        DateTime? endDeliveryTime,
        long? numberOrder)
    {
        if (page < 1)
            throw new NotCorrectDataException(message: "Page value must be greater than 0");

        var manager = await _context.Managers.FindAsync(managerId);

        if (manager == null)
            throw new NotFoundException($"Не найден менеджер с id = {managerId}");

        var orders = await _context.Orders
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                NumberOrder = order.NumberOrder,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();

        if (status != null)
        {
            orders = orders
                .Where(o => o.Status == status)
                .ToList();
        }

        var ordersForManager = new List<OrderDTO>();
        foreach (var order in orders)
        {
            var dishId = await _context.OrdersDishes
                .Where(o => o.OrderId == order.Id)
                .Select(o => o.DishId)
                .FirstOrDefaultAsync();
            var menuId = await _context.MenusDishes
                .Where(o => o.DishId == dishId)
                .Select(o => o.MenuId)
                .FirstOrDefaultAsync();
            var restaurantId = await _context.Menus
                .Where(m => m.Id == menuId)
                .Select(m => m.RestaurantId)
                .FirstOrDefaultAsync();

            if (restaurantId == manager.RestaurantId)
            {
                ordersForManager.Add(order);
            }
        }
        
        if (startOrderTime != null && endOrderTime == null)
        {
            ordersForManager = ordersForManager
                .Where(order => order.OrderTime >= startOrderTime)
                .ToList();
        }

        if (startOrderTime == null && endOrderTime != null)
        {
            ordersForManager = ordersForManager
                .Where(order => order.OrderTime <= endOrderTime)
                .ToList();
        }
        
        if (startOrderTime != null && endOrderTime != null)
        {
            ordersForManager = ordersForManager
                .Where(order => order.OrderTime <= endOrderTime && order.OrderTime >= startOrderTime)
                .ToList();
        }
        
        if (startDeliveryTime != null && endDeliveryTime == null)
        {
            ordersForManager = ordersForManager
                .Where(order => order.DeliveryTime >= startDeliveryTime)
                .ToList();
        }

        if (startDeliveryTime == null && endDeliveryTime != null)
        {
            ordersForManager = ordersForManager
                .Where(order => order.DeliveryTime <= endDeliveryTime)
                .ToList();
        }
        
        if (startDeliveryTime != null && endDeliveryTime != null)
        {
            ordersForManager = ordersForManager
                .Where(order => order.DeliveryTime <= endDeliveryTime && order.DeliveryTime >= startDeliveryTime)
                .ToList();
        }

        ordersForManager = ordersForManager.Where(o => o.NumberOrder.ToString().ToLower().Contains(numberOrder?.ToString().Trim().ToLower() ?? string.Empty)).ToList();

        return ordersForManager;
    }

    
    public async Task ChangeOrderStatus(Guid userId, Guid orderId, OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new NotFoundException($"Заказ с id = {orderId} не найден");
        
        var customer = await _context.Customers.FindAsync(userId);
        var cook = await _context.Cooks.FindAsync(userId);
        var courier = await _context.Couriers.FindAsync(userId);

        var isChangingStatusOrder = false;

        switch (status)
        {
            case OrderStatus.Created:
                throw new NotCorrectDataException("Невозможно вернуться к статусу 'Created'");
            case OrderStatus.Kitchen:
                if (cook != null)
                {
                    if (order.Status == OrderStatus.Created)
                    {
                        order.Status = OrderStatus.Kitchen;
                        await AssignCookToOrder(cook, order);
                        isChangingStatusOrder = true;
                    }
                    else
                    {
                        throw new NotCorrectDataException("Статус 'Kithen' может быть установлен только если текущий статус заказа - 'Created'");
                    }
                }
                else
                {
                    throw new ForbiddenException($"У пользователя с id = {userId} нет прав на изменение статуса заказа на статус 'Kitchen'");
                }
                break;
            case OrderStatus.Packaging:
                if (cook != null)
                {
                    if (order.Status == OrderStatus.Kitchen)
                    {
                        order.Status = OrderStatus.Packaging;
                        isChangingStatusOrder = true;
                    }
                    else
                    {
                        throw new NotCorrectDataException("Статус 'Packaging' может быть установлен только если текущий статус заказа - 'Kitchen'");
                    }
                }
                else
                {
                    throw new ForbiddenException($"У пользователя с id = {userId} нет прав на изменение статуса заказа на статус 'Packaging'");
                }
                break;
            case OrderStatus.WaitingCourier:
                if (cook != null)
                {
                    if (order.Status == OrderStatus.Packaging)
                    {
                        order.Status = OrderStatus.WaitingCourier;
                        isChangingStatusOrder = true;
                    }
                    else
                    {
                        throw new NotCorrectDataException("Статус 'WaitingCourier' может быть установлен только если текущий статус заказа - 'Packaging'");
                    }
                }
                else
                {
                    throw new ForbiddenException($"У пользователя с id = {userId} нет прав на изменение статуса заказа на статус 'WaitingCourier'");
                }
                break;
            case OrderStatus.Delivery:
                if (courier != null)
                {
                    if (order.Status == OrderStatus.WaitingCourier)
                    {
                        order.Status = OrderStatus.Delivery;
                        await AssignCourierToOrder(courier, order);
                        isChangingStatusOrder = true;
                    }
                    else
                    {
                        throw new NotCorrectDataException("Статус 'Delivery' может быть установлен только если текущий статус заказа - 'WaitingCourier'");
                    }
                }
                else
                {
                    throw new ForbiddenException($"У пользователя с id = {userId} нет прав на изменение статуса заказа на статус 'Delivery'");
                }
                break;
            case OrderStatus.Delivered:
                if (courier != null)
                {
                    if (order.Status == OrderStatus.Delivery)
                    {
                        order.Status = OrderStatus.Delivered;
                        isChangingStatusOrder = true;
                    }
                    else
                    {
                        throw new NotCorrectDataException("Статус 'Delivered' может быть установлен только если текущий статус заказа - 'Delivery'");
                    }
                }
                else
                {
                    throw new ForbiddenException($"У пользователя с id = {userId} нет прав на изменение статуса заказа на статус 'Delivery'");
                }
                break;
            case OrderStatus.Cancelled:
                if (courier != null)
                {
                    if (order.Status == OrderStatus.Delivery)
                    {
                        order.Status = OrderStatus.Cancelled;
                        isChangingStatusOrder = true;
                    }
                    else
                    {
                        throw new NotCorrectDataException("Статус 'Cancelled' может быть установлен курьером только если текущий статус заказа - 'Delivery'");
                    }
                }
                else if (customer != null)
                {
                    if (order.Status == OrderStatus.Created)
                    {
                        order.Status = OrderStatus.Cancelled;
                    }
                    else
                    {
                        throw new NotCorrectDataException("Статус 'Cancelled' может быть установлен покупателем только если текущий статус заказа - 'Created'");
                    }
                }
                else
                {
                    throw new ForbiddenException($"У пользователя с id = {userId} нет прав на изменение статуса заказа на статус 'Cancelled'");
                }
                break;
            default:
                throw new InvalidResponseException("Что-то пошло не так с проверкой статуса заказа");
        }
        
        _context.Orders.Attach(order);
        _context.Entry(order).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        if (isChangingStatusOrder)
        {
            _producerService.SendMessage($"Статус заказа с номером {order.NumberOrder} был изменен на {order.Status}");
        }
    }
    
    
    private async Task<double> SumPriceDishes(List<DishInCart> dishesInCart)
    {
        var price = 0.0;

        foreach (var dishInCart in dishesInCart)
        {
            var dish = await _context.Dishes
                .Where(d => d.Id == dishInCart.DishId)
                .FirstOrDefaultAsync();

            if (dish != null) 
                price += dishInCart.Count * dish.Price;
        }
        
        return price;
    }

    private async Task AssignCookToOrder(Cook cook, Order order)
    {
        order.Cook = cook;
        
        _context.Orders.Attach(order);
        _context.Entry(order).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    private async Task AssignCourierToOrder(Courier courier, Order order)
    {
        var activeOrder = await GetActiveOrderForCourier(courier.Id);
        if (activeOrder != null && activeOrder.DeliveryTime >= DateTime.UtcNow)
        {
            throw new InvalidResponseException("Курьер может брать только один заказ в единицу времени для доставки покупателю");
        }
        
        order.Courier = courier;
        
        _context.Orders.Attach(order);
        _context.Entry(order).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}