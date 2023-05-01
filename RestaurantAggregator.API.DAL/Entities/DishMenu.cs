using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregator.API.DAL.Entities;

public class DishMenu
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    [Required]
    [ForeignKey("Menu")]
    public Guid MenuId { get; set; }
    
    public Dish Dish { get; set; }
    
    [Required]
    public Menu Menu { get; set; }
}