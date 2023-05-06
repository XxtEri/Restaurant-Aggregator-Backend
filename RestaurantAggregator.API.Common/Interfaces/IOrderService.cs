using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IOrderService
{
    Task<OrderPageListDTO> GetListLastOrder(string userId, int page);
    Task<List<OrderDTO>> GetActiveOrderForCourier(string userId);
    Task<List<OrderDTO>> GetListLastOrderForCook();

    Task<List<OrderDTO>> GetListActiveOrderForCook(Guid restaurantId);
    Task<List<OrderDTO>> GetListOrderForManager(Guid restaurantId);
    Task<List<OrderDTO>> GetListOrderForCourier();
    Task CreateNewOrder(OrderCreateDTO model);
    Task RepeatLastOrder(Guid orderId, OrderCreateDTO model);
    Task ChangeOrderStatus(Guid orderId, OrderStatus status);
}