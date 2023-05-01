using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class DishPagedListDTO
{
    [MaybeNull]
    public List<DishDTO> Dishes { get; set; }
    
    public PageInfoModelDTO PageInfoModel { get; set; }
}