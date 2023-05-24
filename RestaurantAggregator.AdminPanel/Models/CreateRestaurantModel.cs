using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AdminPanel.Models;

public class CreateRestaurantModel
{
    [Required(ErrorMessage = "Необходимо заполнить название ресторана")]
    [MinLength(1)]
    public string Name { get; set; }
}