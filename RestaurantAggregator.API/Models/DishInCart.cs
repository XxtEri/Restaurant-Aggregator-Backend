using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregatorService.Models;

public class DishInCart
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    //[ForeignKey("Customer")]
    //public Customer Customer { get; set }
    
    [Required]
    public int Count { get; set; }
    
    //[Required]
    //public Customer Customer { get; set; }
    
    [Required]
    public Dish Dish { get; set; }
    
}