using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.DAL.Entities;

public class OrderDish
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }
    
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }

    public Order Order { get; set; }
    
    public Dish Dish { get; set; }
}