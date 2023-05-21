using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregatorService.Models;

public class DishModel
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [MinLength(1)]
    public string? Description { get; set; }
    
    public bool IsVegetarian { get; set; }
    
    [Url]
    public string? Photo { get; set; }
    
    [DefaultValue(0)]
    public double Rating { get; set; }
    
    public DishCategory Category { get; set; }
}