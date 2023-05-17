using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AuthApi.DAL.Etities;

public class Cook
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public User User { get; set; }
    
    public Guid? RestaurantId { get; set; }

}