using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.CommonFiles.Dto;

public class CreateRestaurantDto
{
    [Required]
    public string Name { get; set; }
}