using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AdminPanel.Models;

public class UpdateInfoRestaurant
{
    [Required]
    public string Name { get; set; }
}