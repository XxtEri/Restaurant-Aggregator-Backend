using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregatorService.Models;

public class RestaurantPagedListModel
{
    [MaybeNull]
    public List<RestaurantModel> Restaurants { get; set; }
    
    public PageInfoModel PageInfoModel { get; set; }
}