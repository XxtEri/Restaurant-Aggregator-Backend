using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IOrderService
{
    Task<OrderPageListDTO> GetListLastOrderForCustomer(Guid userId, int page, int? numberOrder, DateTime? startDay, DateTime? endDay);
    Task<List<OrderDTO>> GetListActiveOrderForCustomer(Guid userId);
    Task<OrderDTO> GetConcreteOrder(Guid userId, long numberOrder);
    Task CreateNewOrder(Guid userId, OrderCreateDTO model);
    Task RepeatLastOrder(Guid userId, long numberOrder, OrderCreateDTO model);
    
    Task<List<OrderDTO>> GetOrdersForDelivery();
    Task<OrderDTO?> GetActiveOrderForCourier(Guid courierId);
    Task<List<OrderDTO>> GetLastOrderForCourier(Guid courierId);

    Task<OrderPageListDTO> GetListLastOrderForCook(Guid cookId, int page, int? numberOrder);
    Task<List<OrderDTO>> GetListOrderForCook(Guid cookId);
    Task<List<OrderDTO>> GetListActiveOrderForCook(Guid cookId);
    
    Task<List<OrderDTO>> GetListOrderForManager(
        Guid managerId,
        int page,
        OrderStatus? status,
        DateTime? startOrderTime, 
        DateTime? endOrderTime,
        DateTime? startDeliveryTime, 
        DateTime? endDeliveryTime,
        long? numberOrder
    );
    
    Task ChangeOrderStatus(Guid userId, Guid orderId, OrderStatus status);
}