using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.DAL.Entities;

public class Cook
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public ICollection<Order> Orders { get; set; }

    public Cook()
    {
        Orders = new List<Order>();
    }
}