using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class RestaurantPagedListDto
{
    [MaybeNull]
    public List<RestaurantDTO> Restaurants { get; set; }
    
    public PageInfoModelDTO PageInfoModel { get; set; }
}