using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AuthApi.DAL.Etities;

public class Manager
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public User User { get; set; }
    
    [Required]
    public Guid RestaurantId { get; set; }
}