namespace RestaurantAggregator.AdminPanel.Models;

public class ChangeRestaurantIdViewModel
{
    public Guid UserId { get; set; }
    public Guid? RestaurantId { get; set; }
}