using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregatorService.Models;

public class OrderPageListModel
{
    [MaybeNull]
    public List<OrderModel> Orders { get; set; }
    
    public PageInfoModel PageInfoModel { get; set; }
}