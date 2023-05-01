using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregator.API.DAL.Entities;

public class Order
{
    [Key] public Guid Id { get; set; }

    public DateTime? DeliveryTime { get; set; }

    public DateTime? OrderTime { get; set; }

    [Required] public int Price { get; set; }

    [Required] public OrderStatus Status { get; set; } = OrderStatus.Created;

    [Required]
    [MinLength(2)]
    public string Address { get; set; }
    
    [ForeignKey("DishInCart")]
    public Guid DishInCartId { get; set; }
    
    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }
    
    [ForeignKey("Cook")]
    public Guid? CookId { get; set; }
    
    [ForeignKey("Courier")]
    public Guid CourierId { get; set; }
    
    public DishInCart DishInCart { get; set; }
    
    public Cook Cook { get; set; }
    
    [MaybeNull]
    public Courier Courier { get; set; }
    
    //ссылка на customer

}