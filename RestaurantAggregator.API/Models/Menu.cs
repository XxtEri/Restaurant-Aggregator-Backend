using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregatorService.Models;

public class Menu
{
    [Key]
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
}