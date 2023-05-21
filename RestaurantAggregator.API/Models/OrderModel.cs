using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregatorService.Models;

public class OrderModel
{
    [Key] public Guid Id { get; set; }
    
    public DateTime DeliveryTime { get; set; }
    
    public DateTime OrderTime { get; set; }

    [Required] public double Price { get; set; }

    [Required] [MinLength(1)] public string Address { get; set; }

    [Required] public OrderStatus Status { get; set; }
}