using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.DAL.Entities;

public class DishInCart
{
    [Key]
    public Guid Id { get; set; }
    
    [DefaultValue(0)]
    public int Count { get; set; }
    
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }
    
    public Dish Dish { get; set; }
    
    public Customer Customer { get; set; }
    
    public ICollection<Order> Orders { get; set; }

    public DishInCart()
    {
        Orders = new List<Order>();
    }
}