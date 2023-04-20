using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.Common.DTO;

public class OrderCreateDTO
{
    [Required]
    [DataType(DataType.DateTime)]
    public string DeliveryTime;
    
    [Required]
    public string Address;

}