using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.API.DAL.Entities;
using RestaurantAggregator.AuthApi.Common.Exceptions;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class OrderService: IOrderService
{
    private readonly ApplicationDBContext _context;

    public OrderService(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<OrderPageListDTO> GetListLastOrder(string userId, int page, DateTime? startDay, DateTime? endDay)
    {
        if (page < 1)
        {
            throw new NotCorrectDataException(message: "Page value must be greater than 0");
        }
        
        var orders = await _context.Orders
            .Where(order => order.CustomerId.ToString() == userId && order.Status == OrderStatus.Delivered)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status
            })
            .ToListAsync();
        
        if (!orders.Any())
        {
            throw new NotFoundElementException("У вас пока еще нет завершенных заказов");
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

        var pageSize = 5;
        var countDishes = orders.Count();
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

    public async Task<OrderDTO> GetConcreteOrder(string userId, string numberOrder)
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
            throw new NotFoundElementException($"Заказа по номеру {numberOrder} не найдено");
        }
        
        return order;
    }

    public async Task CreateNewOrder(OrderCreateDTO model)
    {
        await _context.AddAsync(new Order
        {
            NumberOrder = "",
            DeliveryTime = model.DeliveryTime,
            Status = OrderStatus.Created,
            Address = model.Address
        });
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

    public async Task<List<OrderDTO>> GetActiveOrderForCourier(string userId)
    {
        var orders = await _context.Orders
            .Where(order => order.CustomerId.ToString() == userId && order.Status != OrderStatus.Delivered)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
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
}