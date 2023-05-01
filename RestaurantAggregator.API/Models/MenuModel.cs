using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregatorService.Models;

public class MenuModel
{
    [Key]
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
}