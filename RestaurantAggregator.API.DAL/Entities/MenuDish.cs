using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregator.API.DAL.Entities;

public class MenuDish
{
    [ForeignKey("Dish")]
    [Required]
    public Guid DishId { get; set; }
    
    [ForeignKey("Menu")]
    [Required]
    public Guid MenuId { get; set; }
    
    [Required]
    public Dish Dish { get; set; }
    
    [Required]
    public Menu Menu { get; set; }
}