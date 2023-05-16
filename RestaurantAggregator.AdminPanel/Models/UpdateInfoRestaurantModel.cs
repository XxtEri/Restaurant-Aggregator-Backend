using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AdminPanel.Models;

public class UpdateInfoRestaurantModel
{
    [Required]
    public string Name { get; set; }
}