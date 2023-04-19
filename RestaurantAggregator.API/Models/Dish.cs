using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregatorService.Enum;

namespace RestaurantAggregatorService.Models;

public class Dish
{
    [Key]
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [MaybeNull]
    public string Description { get; set; }
    
    [MaybeNull]
    public bool IsVegetarian { get; set; }
    
    [Url]
    [MaybeNull]
    public string Photo { get; set; }
    
    [MaybeNull]
    public double Rating { get; set; }
    
    public DishCategory Category { get; set; }
}