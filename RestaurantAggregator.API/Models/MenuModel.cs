using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregatorService.Models;

public class MenuModel
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    public List<DishModel> Dishes { get; set; }
}