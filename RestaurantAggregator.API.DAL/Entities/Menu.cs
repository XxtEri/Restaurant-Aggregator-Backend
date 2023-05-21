using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAggregator.API.DAL.Entities;

public class Menu
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [ForeignKey("Restaurant")]
    public Guid RestaurantId { get; set; }
    
    public Restaurant Restaurant { get; set; }
    
    public ICollection<MenuDish> MenusDishes { get; set; }
    
    public Menu()
    {
        MenusDishes = new List<MenuDish>();
    }
}