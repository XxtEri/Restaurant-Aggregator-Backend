using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregatorService.Models;

public class RestaurantModel
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    [MaybeNull]
    public List<MenuModel> Menu { get; set; }
    
    [MaybeNull]
    public ICollection<CookModel> Cooks { get; set; }
    
    [MaybeNull]
    public ICollection<ManagerModel> Managers { get; set; }

    public RestaurantModel()
    {
        Cooks = new List<CookModel>();
        Managers = new List<ManagerModel>();
    }
}