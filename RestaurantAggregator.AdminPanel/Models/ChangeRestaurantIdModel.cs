using RestaurantAggregator.CommonFiles;

namespace RestaurantAggregator.AdminPanel.Models;

public class ChangeRestaurantIdModel
{
    public Guid UserId { get; set; }
    public Guid RestaurantId { get; set; }
}