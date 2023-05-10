using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.Common.DTO;

public class OrderCreateDTO
{
    [Required]
    public DateTime DeliveryTime;
    
    [Required]
    public string Address;

}