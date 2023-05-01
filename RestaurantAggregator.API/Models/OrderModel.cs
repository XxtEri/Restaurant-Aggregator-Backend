using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregatorService.Models;

public class OrderModel
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
    public CookModel Cook { get; set; }
    
    [MaybeNull]
    public CourierModel Courier { get; set; }
    
    [MaybeNull]
    public ICollection<DishInCartModel> DishInCarts { get; set; }

    public OrderModel()
    {
        DishInCarts = new List<DishInCartModel>();
    }
}