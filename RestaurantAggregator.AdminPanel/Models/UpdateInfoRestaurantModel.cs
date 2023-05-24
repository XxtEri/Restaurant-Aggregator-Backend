using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AdminPanel.Models;

public class UpdateInfoRestaurantModel
{
    [Required]
    public Guid RestaurantId { get; set; }
    
    [Required(ErrorMessage = "Необходимо заполнить название ресторана")]
    [MinLength(1)]
    public string Name { get; set; }
}