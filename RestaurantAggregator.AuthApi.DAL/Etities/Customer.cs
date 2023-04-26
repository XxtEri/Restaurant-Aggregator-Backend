using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.AuthApi.DAL.Etities;

public class Customer : User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public User User { get; set; }

    [Required] 
    public string Address { get; set; }
}