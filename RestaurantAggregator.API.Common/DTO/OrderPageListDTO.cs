using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class OrderPageListDTO
{
    [MaybeNull]
    public List<OrderDTO> Orders { get; set; }
    
    public PageInfoModelDTO PageInfoModel { get; set; }
}