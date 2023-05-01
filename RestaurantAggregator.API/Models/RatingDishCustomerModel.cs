using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregatorService.Models;

public class RatingDishCustomer
{
    [Key]
    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }
    
    [Key]
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    [Required]
    public double Rating { get; set; }
    
    // [Required]
    // public Customer Customer { get; set; }
    
    [Required]
    public Dish Dish { get; set; }
}