using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class RestaurantPagedListDTO
{
    [MaybeNull]
    public List<RestaurantDTO> Restaurants { get; set; }
}