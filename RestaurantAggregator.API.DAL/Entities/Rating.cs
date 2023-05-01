using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.DAL.Entities;

public class Rating
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public int Value { get; set; }
    
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }
    
    public Dish Dish { get; set; }
    
    //ссылка на customer
}