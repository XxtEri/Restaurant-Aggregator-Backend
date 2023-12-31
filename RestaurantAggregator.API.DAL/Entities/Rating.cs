using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.DAL.Entities;

public class Rating
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public int Value { get; set; }
    
    public Guid DishId { get; set; }

    public Guid CustomerId { get; set; }
}