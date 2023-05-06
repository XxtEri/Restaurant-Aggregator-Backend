using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.DAL.Entities;

public class Courier
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public ICollection<Order> Orders { get; set; }

    public Courier()
    {
        Orders = new List<Order>();
    }
}