using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.Enums;

namespace RestaurantAggregator.API.DAL.Entities;

public class Dish
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [MaybeNull]
    [MinLength(1)]
    public string Description { get; set; }
    
    [Required]
    public bool IsVegetarian { get; set; }
    
    [MaybeNull]
    [Url]
    public string Photo { get; set; }
    
    [DefaultValue(0)]
    public int Rating { get; set; }
    
    public DishCategory Category { get; set; }

    public ICollection<MenuDish> MenusDishes { get; set; }
    
    public ICollection<DishInCart> DishInCarts { get; set; }
    
    public Dish()
    {
        MenusDishes = new List<MenuDish>();
        DishInCarts = new List<DishInCart>();
    }
}