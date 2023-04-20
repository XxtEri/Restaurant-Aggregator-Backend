using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregator.API.Common.DTO;

public class OrderDTO
{
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
    public CookDTO Cook { get; set; }
    
    [MaybeNull]
    public CourierDTO Courier { get; set; }
    
    // [MaybeNull]
    // public ICollection<DishInCart> DishInCarts { get; set; }
}