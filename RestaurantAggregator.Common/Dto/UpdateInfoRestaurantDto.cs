using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.CommonFiles.Dto;

public class UpdateInfoRestaurantDto
{
    [Required]
    public string Name { get; set; }
}