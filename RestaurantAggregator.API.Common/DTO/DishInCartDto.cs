using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.Common.DTO;

public class DishInCartDto
{
    public Guid Id { get; set; }

    [Required]
    public int Count { get; set; }
    
    [Required]
    public DishDTO Dish { get; set; }
}