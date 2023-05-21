using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IOrderService
{
    Task<OrderPageListDTO> GetListLastOrder(Guid userId, int page, DateTime? startDay, DateTime? endDay);
    Task<OrderDTO> GetConcreteOrder(Guid userId, string numberOrder);
    Task CreateNewOrder(Guid userId, OrderCreateDTO model);
    Task RepeatLastOrder(Guid orderId, OrderCreateDTO model);
    
    Task<List<OrderDTO>> GetActiveOrderForCourier(Guid userId);
    Task<List<OrderDTO>> GetListOrderForCourier();
    
    Task<List<OrderDTO>> GetListLastOrderForCook();
    Task<List<OrderDTO>> GetListActiveOrderForCook(Guid restaurantId);
    
    Task<List<OrderDTO>> GetListOrderForManager(Guid restaurantId);
    
    Task ChangeOrderStatus(Guid orderId, OrderStatus status);
}