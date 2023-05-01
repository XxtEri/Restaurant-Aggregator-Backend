using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class OrderPageListDTO
{
    [MaybeNull]
    public List<OrderDTO> Dishes { get; set; }
    
    public PageInfoModelDTO PageInfoModel { get; set; }
}