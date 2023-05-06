using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregator.API.Common.DTO;

public class DishDTO
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [MaybeNull]
    [MinLength(1)]
    public string Description { get; set; }
    
    [MaybeNull]
    public bool IsVegetarian { get; set; }
    
    [Url]
    [MaybeNull]
    public string Photo { get; set; }
    
    [DefaultValue(0)]
    public double Rating { get; set; }
    
    public DishCategory Category { get; set; }
}