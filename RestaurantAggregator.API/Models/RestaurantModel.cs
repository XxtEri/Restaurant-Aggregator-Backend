using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregatorService.Models;

public class Restaurant
{
    [Key]
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    [MaybeNull]
    public List<Menu> Menu { get; set; }
    
    [MaybeNull]
    public ICollection<Cook> Cooks { get; set; }
    
    [MaybeNull]
    public ICollection<Manager> Managers { get; set; }

    public Restaurant()
    {
        Cooks = new List<Cook>();
        Managers = new List<Manager>();
    }
}