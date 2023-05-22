using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.API.Common.DTO;

public class MenuDTO
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [Required]
    public string Name { get; set; }
    
    public List<DishDTO> Dishes { get; set; }
}