using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IOrderService
{
    Task<OrderPageListDTO> GetListLastOrder(string userId, int page, DateTime? startDay, DateTime? endDay);
    Task<OrderDTO> GetConcreteOrder(string userId, string numberOrder);
    Task CreateNewOrder(OrderCreateDTO model);
    Task RepeatLastOrder(Guid orderId, OrderCreateDTO model);
    
    Task<List<OrderDTO>> GetActiveOrderForCourier(string userId);
    Task<List<OrderDTO>> GetListOrderForCourier();
    
    Task<List<OrderDTO>> GetListLastOrderForCook();
    Task<List<OrderDTO>> GetListActiveOrderForCook(Guid restaurantId);
    
    Task<List<OrderDTO>> GetListOrderForManager(Guid restaurantId);
    
    Task ChangeOrderStatus(Guid orderId, OrderStatus status);
}