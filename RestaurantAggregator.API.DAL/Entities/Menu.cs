using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.DAL.Entities;

public class Menu
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public String Name { get; set; }
}