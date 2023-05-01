using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.DAL.Entities;

public class Dish
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [MaybeNull]
    [MinLength(1)]
    public string Description { get; set; }
    
    [Required]
    public bool IsVegetarian { get; set; }
    
    [MaybeNull]
    [Url]
    public string Photo { get; set; }
    
}