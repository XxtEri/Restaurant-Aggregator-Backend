using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.CommonFiles.Enums;

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
    
    public ICollection<OrderDish> OrderDishes { get; set; }
    
    public Dish()
    {
        OrderDishes = new List<OrderDish>();
        MenusDishes = new List<MenuDish>();
        DishInCarts = new List<DishInCart>();
    }
}