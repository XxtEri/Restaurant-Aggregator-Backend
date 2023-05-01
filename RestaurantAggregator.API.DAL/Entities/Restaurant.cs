using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.API.DAL.Entities;

public class Restaurant
{
    [Key] 
    public Guid Id { get; set; }
    
    [Required]
    public String Name { get; set; }
    
    public ICollection<Menu> Menus { get; set; }
    
    [ForeignKey("Cook")]
    public Guid? CookId { get; set; }
    
    [ForeignKey("Manager")]
    public Guid? ManagerId { get; set; }

    [MaybeNull]
    public Cook Cook { get; set; }
    
    [MaybeNull]
    public Manager Manager { get; set; }

    public Restaurant()
    {
        Menus = new List<Menu>();
    }
}