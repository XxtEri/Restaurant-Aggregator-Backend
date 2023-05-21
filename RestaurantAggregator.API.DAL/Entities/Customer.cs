using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.DAL.Entities;

public class Customer
{
    [Key]
    public Guid Id { get; set; }
    public ICollection<DishInCart> DishInCarts { get; set; }
    
    public Customer()
    {
        DishInCarts = new List<DishInCart>();
    }
}