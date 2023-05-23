using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.API.Common.DTO;

public class OrderDTO
{
    public Guid Id { get; set; }
    
    [Required]
    public long NumberOrder { get; set; }
    
    public DateTime DeliveryTime { get; set; }
    
    public DateTime OrderTime { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Address { get; set; }
    
    [Required]
    public OrderStatus Status { get; set; }
}