using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.AuthApi.Common.Exceptions;

namespace RestaurantAggregator.API.BL.Services;

public class OrderService: IOrderService
{
    private readonly ApplicationDBContext _context;

    public OrderService(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<OrderPageListDTO> GetListLastOrder(string userId, int page)
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

    public Task<List<OrderDTO>> GetListOrderForCourier()
    {
        throw new NotImplementedException();
    }

    Task IOrderService.CreateNewOrder(OrderCreateDTO model)
    {
        return CreateNewOrder(model);
    }

    public Task RepeatLastOrder(Guid orderId, OrderCreateDTO model)
    {
        throw new NotImplementedException();
    }

    public Task ChangeOrderStatus(Guid orderId, OrderStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDTO>> CreateNewOrder(OrderCreateDTO model)
    {
        throw new NotImplementedException();
    }
}