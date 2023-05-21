using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregatorService.Models;

public class DishInCartModel
{
    public Guid Id { get; set; }

    [Required]
    public int Count { get; set; }
    
    [Required]
    public DishModel Dish { get; set; }
    
}