using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregatorService.Models;

public class MenuDishModel
{
    [Key]
    [ForeignKey("Menu")]
    public Guid MenuId { get; set; }

    [Key] 
    [ForeignKey("Dish")] 
    public Guid DishId { get; set; }
}