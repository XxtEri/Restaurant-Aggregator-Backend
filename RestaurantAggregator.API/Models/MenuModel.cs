using System.ComponentModel.DataAnnotations;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.DAL.Entities;

namespace RestaurantAggregatorService.Models;

public class MenuModel
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    public List<DishModel> Dishes { get; set; }

    public MenuModel()
    {
        Dishes = new List<DishModel>();
    }
}