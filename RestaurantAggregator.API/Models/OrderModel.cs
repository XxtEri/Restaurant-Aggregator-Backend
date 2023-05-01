using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregatorService.Models;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    [DataType(DataType.DateTime)]
    [Required]
    public string DeliveryTime { get; set; }
    
    [DataType(DataType.DateTime)]
    [Required]
    public string Ordertime { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Address { get; set; }
    
    [Required]
    public OrderStatus Status { get; set; }
    
    [MaybeNull]
    public Cook Cook { get; set; }
    
    [MaybeNull]
    public Courier Courier { get; set; }
    
    [MaybeNull]
    public ICollection<DishInCart> DishInCarts { get; set; }

    public Order()
    {
        DishInCarts = new List<DishInCart>();
    }
}