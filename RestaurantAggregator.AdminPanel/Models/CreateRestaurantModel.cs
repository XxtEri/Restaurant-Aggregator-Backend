using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AdminPanel.Models;

public class CreateRestaurantModel
{
    [Required]
    public string Name { get; set; }
}