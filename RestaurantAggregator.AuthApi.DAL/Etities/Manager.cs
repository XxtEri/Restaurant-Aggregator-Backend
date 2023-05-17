using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.AuthApi.DAL.Etities;

public class Manager
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public User User { get; set; }
    
    public Guid? RestaurantId { get; set; }
}