using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregatorService.Models;

public class DishPagedListModel
{
    [MaybeNull]
    public List<DishModel> Dishes { get; set; }
    
    public PageInfoModel PageInfoModel { get; set; }
}