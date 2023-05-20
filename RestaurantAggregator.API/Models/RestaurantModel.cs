using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregatorService.Models;

public class RestaurantModel
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    [MaybeNull]
    public List<MenuModel> Menus { get; set; }
}